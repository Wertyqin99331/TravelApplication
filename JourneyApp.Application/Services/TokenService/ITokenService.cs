using JourneyApp.Core.Models.User;
using Microsoft.AspNetCore.Identity;

namespace JourneyApp.Application.Services.TokenService;

public interface ITokenService
{
    string GenerateToken(User user, IList<string> roles);
}