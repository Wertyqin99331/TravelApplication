using CSharpFunctionalExtensions;
using JourneyApp.Application.CommonTypes;
using JourneyApp.Application.Options;
using JourneyApp.Application.Services.Authentication;
using JourneyApp.Application.Services.Authentication.Dto;
using JourneyApp.Application.Services.FileStorageService;
using JourneyApp.Application.Services.TokenService;
using JourneyApp.Application.Services.UserService;
using JourneyApp.Core.CommonTypes;
using JourneyApp.Core.Models.User;
using JourneyApp.Core.ValueObjects.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace JourneyApp.Infrastructure.Services;

public class AuthenticationService(
    UserManager<User> userManager,
    SignInManager<User> signInManager,
    ITokenService tokenService,
    IUserService userService,
    IFileStorageService minioService,
    IOptions<MinioOptions> minioOptions) : IAuthenticationService
{
    private readonly UserManager<User> userManager = userManager;
    private readonly SignInManager<User> signInManager = signInManager;
    private readonly ITokenService tokenService = tokenService;
    private readonly IUserService userService = userService;
    private readonly IFileStorageService minioService = minioService;
    private readonly IOptions<MinioOptions> minioOptions = minioOptions;

    public async Task<UnitResult<ApplicationError>> RegisterAsync(RegisterBody body)
    {
        if (await userManager.FindByEmailAsync(body.Email) is not null)
        {
            return new ApplicationError("Пользователь с таким email уже существует");
        }

        var userResult = User.Create(body.Email, body.Name, body.Surname);
        if (userResult.IsFailure)
            return userResult.Error;

        var signUpResult = await userManager.CreateAsync(userResult.Value, body.Password);
        if (!signUpResult.Succeeded)
            return new ApplicationError(string.Join(';', signUpResult.Errors.Select(e => e.Description)));
        
        return UnitResult.Success<ApplicationError>();
    }

    public async Task<Result<LoginResult, ApplicationError>> LoginAsync(LoginBody body)
    {
        var user = await userManager.FindByEmailAsync(body.Email);
        if (user is null)
            return new ApplicationError("Пользователь не найден");

        var signInResult = await signInManager
            .CheckPasswordSignInAsync(user, body.Password, false);
        if (!signInResult.Succeeded)
            return new ApplicationError("Неверный пароль");

        var roles = await userManager.GetRolesAsync(user);
        var token = tokenService.GenerateToken(user, roles);
        return new LoginResult(token, roles.Contains(UserRole.Admin.ToString()) ? UserRole.Admin : UserRole.User);
    }

    public async Task<UnitResult<ApplicationError>> UpdateProfileAsync(UpdateProfileBody body)
    {
        var userResult = await userService.GetUserFromTokenAsync();
        if (userResult.IsFailure)
            return userResult.Error;

        var user = userResult.Value;
        
        var nameResult = Name.Create(body.Name);
        if (nameResult.IsFailure)
            return nameResult.Error;
        
        var surnameResult = Name.Create(body.Surname);
        if (surnameResult.IsFailure)
            return surnameResult.Error;

        user.Name = nameResult.Value;
        user.Surname = surnameResult.Value;
        user.Email = body.Email;
        user.UserName = body.Email;

        if (body.Avatar != null)
        {
            var fileName = $"{user.Id}{Path.GetExtension(body.Avatar.FileName)}";
            var uploadResult = await minioService.UploadFileAsync(
                body.Avatar,
                fileName,
                minioOptions.Value.DefaultBucket,
                "avatars");

            if (uploadResult.IsFailure)
                return uploadResult.Error;

            user.AvatarUrl = uploadResult.Value;
        }

        var updateResult = await userManager.UpdateAsync(user);
        if (!updateResult.Succeeded)
            return new ApplicationError(string.Join(";", updateResult.Errors.Select(e => e.Description)));

        if (!string.IsNullOrEmpty(body.Password))
        {
            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            var resetResult = await userManager.ResetPasswordAsync(user, token, body.Password);
            if (!resetResult.Succeeded)
                return new ApplicationError(string.Join(";", resetResult.Errors.Select(e => e.Description)));
        }

        return UnitResult.Success<ApplicationError>();
    }

    public async Task<Result<GetProfileResult, ApplicationError>> GetProfileAsync()
    {
        var userResult = await userService.GetUserFromTokenAsync();
        if (userResult.IsFailure)
            return userResult.Error;

        var user = userResult.Value;
        var profile = new UserProfile(user.Email!, user.Name.Value, user.Surname.Value, user.AvatarUrl);
        return new GetProfileResult(profile);
    }
}