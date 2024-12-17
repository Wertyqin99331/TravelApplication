using JourneyApp.Application;
using JourneyApp.Application.CommonTypes;
using JourneyApp.Infrastructure;
using JourneyApp.Infrastructure.Database.Helpers;
using JourneyApp.WebApi.Authentication;
using JourneyApp.WebApi.Endpoints.Authentication;
using JourneyApp.WebApi.Endpoints.Trip;
using JourneyApp.WebApi.Filters;
using JourneyApp.WebApi.GlobalExceptionHandler;
using JourneyApp.WebApi.Mapping;
using JourneyApp.WebApi.Swagger;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger();

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

builder.Services.AddGlobalExceptionHandler();

builder.Services.AddAuthenticationAndAuthorization(builder.Configuration);

builder.Services.AddAntiforgery(options =>
{
    options.HeaderName = "X-XSRF-TOKEN";
    options.Cookie.Name = "XSRF-TOKEN";
    options.Cookie.SameSite = SameSiteMode.None;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});

builder.Services.AddCors();

builder.Services.AddAndConfigureMapster();

var app = builder.Build();

app.UseCors(corsOptions =>
{
    corsOptions.AllowAnyHeader()
        .AllowAnyMethod()
        .SetIsOriginAllowed(_ => true)
        .AllowCredentials();
});

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ApplyMigrations();
    await app.SeedAdminRoleAndUser();
}

app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();

var prefix = app.MapGroup("/api");
prefix.MapAuthenticationEndpoints();
prefix.MapTripEndpoints();

prefix.MapGet("/", () => new UserProfile("kek@mail.ru", "Ilai", "Surname", null))
    .AddEndpointFilter<ResponseFieldsFilter>();

app.Run();