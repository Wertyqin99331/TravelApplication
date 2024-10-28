using Microsoft.OpenApi.Models;

namespace JourneyApp.WebApi.Swagger;

public static class SwaggerStartup
{
    public static void AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.MapType<DateOnly>(() => new OpenApiSchema()
            {
                Type = "string",
                Format = "date"
            });

            options.SchemaFilter<EnumSchemaFilter>();
        });
    }
}