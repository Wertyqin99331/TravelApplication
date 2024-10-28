using CSharpFunctionalExtensions;
using JourneyApp.Application.Services.Authentication;
using JourneyApp.Application.Services.Authentication.Dto;
using JourneyApp.Application.Services.TokenService;
using JourneyApp.Core.CommonTypes;
using JourneyApp.Core.Models.User;
using JourneyApp.Core.ValueObjects.User;
using Microsoft.AspNetCore.Identity;

namespace JourneyApp.Infrastructure.Services;

public class AuthenticationService(SignInManager<User> signInManager, UserManager<User> userManager, ITokenService tokenService): IAuthenticationService
{
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
}