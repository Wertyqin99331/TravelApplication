namespace JourneyApp.WebApi.GlobalExceptionHandler;

public static class GlobalExceptionHandlerStartup
{
    public static void AddGlobalExceptionHandler(this IServiceCollection services)
    {
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();
    }
}