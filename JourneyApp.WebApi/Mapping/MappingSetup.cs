using Mapster;

namespace JourneyApp.WebApi.Mapping;

public static class MappingSetup
{
    public static void AddAndConfigureMapster(this IServiceCollection services)
    {
        services.AddMapster();

        TypeAdapterConfig.GlobalSettings.RequireDestinationMemberSource = true;
        TypeAdapterConfig.GlobalSettings.Scan(typeof(MappingSetup).Assembly, Core.CurrentAssembly.Current, Application.CurrentAssembly.Current,
            Infrastructure.CurrentAssembly.Current);
    }
}