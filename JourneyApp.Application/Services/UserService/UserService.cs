using System.IdentityModel.Tokens.Jwt;
using CSharpFunctionalExtensions;
using JourneyApp.Application.Services.TokenService;
using JourneyApp.Core.CommonTypes;
using JourneyApp.Core.Models.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace JourneyApp.Application.Services.UserService;

public class UserService(ITokenService tokenService, UserManager<User> userManager)
{
    public async Task<Result<User, ApplicationError>> GetUserFromTokenAsync()
    {
        var userIdResult = tokenService
            .ReadValueFromClaims(JwtRegisteredClaimNames.Jti);

        if (userIdResult.IsFailure)
        {
            return userIdResult.Error;
        }

        var user = await userManager
            .FindByIdAsync(userIdResult.Value);
        return user ?? Result.Failure<User, ApplicationError>(new ApplicationError("Пользователь не найден"));
    }
}