using Microsoft.Extensions.DependencyInjection;

namespace JourneyApp.Application.Extensions;

public static class OptionExtensions
{
    public static void AddOptionsWithValidation<TOptions>(this IServiceCollection services, string sectionPath)
        where TOptions : class
    {
        services.AddOptions<TOptions>()
            .BindConfiguration(sectionPath)
            .ValidateDataAnnotations()
            .ValidateOnStart();
    }
}