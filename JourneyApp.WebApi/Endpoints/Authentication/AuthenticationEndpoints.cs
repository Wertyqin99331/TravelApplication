using CSharpFunctionalExtensions;
using JourneyApp.Application.Services.Authentication;
using JourneyApp.Application.Services.Authentication.Dto;
using JourneyApp.Core.CommonTypes;
using JourneyApp.WebApi.Endpoints.Authentication.Dto;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace JourneyApp.WebApi.Endpoints.Authentication;

public static class AuthenticationEndpoints
{
    private const string Tag = "Authentication endpoints";
    
    public static void MapAuthenticationEndpoints(this IEndpointRouteBuilder app)
    {
        var endpoint = app
            .MapGroup("/authentication");

        endpoint
            .MapPost("/register", Register)
            .Accepts<RegisterRequest>("application/json")
            .Produces<NoContentResult>(StatusCodes.Status204NoContent)
            .Produces<ApplicationError>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        endpoint
            .MapPost("/login", Login)
            .Accepts<LoginRequest>("application/json")
            .Produces<LoginResponse>()
            .Produces<ApplicationError>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);
    }

    private static async Task<IResult> Register([FromBody] RegisterRequest request,
        IAuthenticationService authenticationService, IMapper mapper)
    {
        var registerResult = await authenticationService.RegisterAsync(mapper.Map<RegisterBody>(request));
        return registerResult.Match(Results.NoContent, Results.BadRequest);
    }

    private static async Task<IResult> Login([FromBody] LoginRequest request, IMapper mapper,
        IAuthenticationService authenticationService)
    {
        var loginResult = await authenticationService.LoginAsync(mapper.Map<LoginBody>(request));
        return loginResult.Match(res => Results.Ok(mapper.Map<LoginResponse>(res)),
            Results.BadRequest);
    }
}