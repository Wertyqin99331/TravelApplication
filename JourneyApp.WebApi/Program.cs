using JourneyApp.Application;
using JourneyApp.Infrastructure;
using JourneyApp.Infrastructure.Database.Helpers;
using JourneyApp.WebApi.Authentication;
using JourneyApp.WebApi.Endpoints.Authentication;
using JourneyApp.WebApi.Endpoints.Trip;
using JourneyApp.WebApi.GlobalExceptionHandler;
using JourneyApp.WebApi.Mapping;
using JourneyApp.WebApi.Swagger;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger();

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

builder.Services.AddGlobalExceptionHandler();

builder.Services.AddAuthenticationAndAuthorization(builder.Configuration);

builder.Services.AddAndConfigureMapster();

var app = builder.Build();

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


var prefix = app.MapGroup("/api");
prefix.MapAuthenticationEndpoints();
prefix.MapTripEndpoints();

app.Run();
