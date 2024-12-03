using JourneyApp.Application.Extensions;
using JourneyApp.Application.Options;
using JourneyApp.Application.Services.TokenService;
using JourneyApp.Application.Services.TripService;
using JourneyApp.Application.Services.UserService;
using Microsoft.Extensions.DependencyInjection;

namespace JourneyApp.Application;

public static class DependencyInjection
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<TripService>();
        services.AddScoped<ITokenService, TokenService>();
        
        services.AddOptionsWithValidation<JwtOptions>(JwtOptions.SECTION_NAME);
    }
}