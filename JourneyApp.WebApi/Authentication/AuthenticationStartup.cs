using System.Data;
using System.Text;
using JourneyApp.Application.Extensions;
using JourneyApp.Application.Options;
using JourneyApp.Core.ValueObjects.User;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace JourneyApp.WebApi.Authentication;

public static class AuthenticationStartup
{
    public static void AddAuthenticationAndAuthorization(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtTokenOptions = configuration
            .GetSection(JwtOptions.SECTION_NAME)
            .Get<JwtOptions>() ?? throw new NoNullAllowedException("Не задана секция JwtTokenOptions");

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
            {
                ValidateIssuer = jwtTokenOptions.ValidateAudience,
                ValidateAudience = jwtTokenOptions.ValidateAudience,
                ValidateLifetime = jwtTokenOptions.ValidateLifetime,
                ValidateIssuerSigningKey = jwtTokenOptions.ValidateIssuerSigningKey,
                ValidIssuer = jwtTokenOptions.Issuer,
                ValidAudience = jwtTokenOptions.Audience,
                IssuerSigningKey =
                    new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtTokenOptions.Secret))
            };
        });

        services.AddAuthorizationBuilder()
            .AddDefaultPolicy(AuthorizationSettings.DEFAULT_AUTHORIZATION_POLICY_NAME, policy =>
            {
                policy.RequireAuthenticatedUser();
            })
            .AddPolicy(AuthorizationSettings.ADMIN_AUTHORIZATION_POLICY_NAME, policy =>
            {
                policy.RequireAuthenticatedUser()
                    .RequireRole(UserRole.Admin.ToString());
            });
    }
}