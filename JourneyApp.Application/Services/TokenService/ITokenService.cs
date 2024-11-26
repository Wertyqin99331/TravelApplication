using CSharpFunctionalExtensions;
using JourneyApp.Core.CommonTypes;
using JourneyApp.Core.Models.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace JourneyApp.Application.Services.TokenService;

public interface ITokenService
{
    string GenerateToken(User user, IList<string> roles);
    Result<string, ApplicationError> ReadValueFromClaims(string claimType);
}