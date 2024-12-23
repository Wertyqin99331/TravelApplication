using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CSharpFunctionalExtensions;
using JourneyApp.Application.Options;
using JourneyApp.Core.CommonTypes;
using JourneyApp.Core.Models.User;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace JourneyApp.Application.Services.TokenService;

public class TokenService(IOptions<JwtOptions> jwtOptions, IHttpContextAccessor httpContextAccessor): ITokenService
{
    public string GenerateToken(User user, IList<string> roles)
    {
        var claims = new List<Claim>()
        {
            new(JwtRegisteredClaimNames.Jti, user.Id.ToString()),
        };
        
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
        var token = GenerateToken(claims);

        return token;
    }

    public Result<string, ApplicationError> ReadValueFromClaims(string claimType)
    {
        if (httpContextAccessor.HttpContext is null)
            return new ApplicationError("Http контекст не найден");

        var claimValue = httpContextAccessor.HttpContext.User.FindFirstValue(claimType);
        return claimValue ?? Result.Failure<string, ApplicationError>(new ApplicationError("Не удалось прочитать значение claim"));
    }

    private string GenerateToken(IEnumerable<Claim> claims)
    {
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Value.Secret));

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = jwtOptions.Value.Issuer,
            Audience = jwtOptions.Value.Audience,
            Expires = DateTime.UtcNow.AddHours(jwtOptions.Value.DurationInHours),
            SigningCredentials = new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256),
            Subject = new ClaimsIdentity(claims)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}