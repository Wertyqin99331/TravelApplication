using System.IdentityModel.Tokens.Jwt;
using CSharpFunctionalExtensions;
using JourneyApp.Application.Services.TokenService;
using JourneyApp.Application.Services.UserService;
using JourneyApp.Core.CommonTypes;
using JourneyApp.Core.Models.User;
using Microsoft.AspNetCore.Identity;

namespace JourneyApp.Infrastructure.Services;

public class UserService(ITokenService tokenService, UserManager<User> userManager) : IUserService
{
    private readonly ITokenService tokenService = tokenService;
    private readonly UserManager<User> userManager = userManager;

    public async Task<Result<User, ApplicationError>> GetUserFromTokenAsync()
    {
        var userIdResult = tokenService.ReadValueFromClaims(JwtRegisteredClaimNames.Jti);
        if (userIdResult.IsFailure)
            return userIdResult.Error;

        var user = await userManager.FindByIdAsync(userIdResult.Value);
        if (user is null)
            return new ApplicationError("User not found");

        return user;
    }
}
