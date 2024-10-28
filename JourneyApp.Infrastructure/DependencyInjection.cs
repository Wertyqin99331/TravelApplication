using System.Data;
using JourneyApp.Application.Extensions;
using JourneyApp.Application.Interfaces;
using JourneyApp.Application.Services.Authentication;
using JourneyApp.Core.Models.User;
using JourneyApp.Infrastructure.Database;
using JourneyApp.Infrastructure.Options;
using JourneyApp.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JourneyApp.Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<JourneyAppDbContext>();
        services.AddScoped<IJourneyAppDbContext>(provider => provider.GetRequiredService<JourneyAppDbContext>());
        
        services.AddOptionsWithValidation<IdentityPasswordOptions>(IdentityPasswordOptions.SECTION_NAME);
        var identityPasswordOptions = configuration
            .GetSection(IdentityPasswordOptions.SECTION_NAME)
            .Get<IdentityPasswordOptions>() ?? throw new NoNullAllowedException("Не задана секция IdentityPasswordOptions");

        services.AddIdentity<User, IdentityRole<Guid>>(options =>
            {
                options.Password.RequireDigit = identityPasswordOptions.RequireDigit;
                options.Password.RequireLowercase = identityPasswordOptions.RequireLowercase;
                options.Password.RequireUppercase = identityPasswordOptions.RequireUppercase;
                options.Password.RequireNonAlphanumeric = identityPasswordOptions.RequireNonAlphanumeric;
                options.Password.RequiredLength = identityPasswordOptions.RequiredLength;
                options.User.RequireUniqueEmail = identityPasswordOptions.RequireUniqueEmail;
            })
            .AddUserManager<UserManager<User>>()
            .AddEntityFrameworkStores<JourneyAppDbContext>();

        services.AddScoped<IAuthenticationService, AuthenticationService>();
    }
}