using CSharpFunctionalExtensions;
using JourneyApp.Application.Services.Authentication;
using JourneyApp.Application.Services.Authentication.Dto;
using JourneyApp.Core.CommonTypes;
using JourneyApp.WebApi.Endpoints.Authentication.Dto;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace JourneyApp.WebApi.Endpoints.Authentication;

public static class AuthenticationEndpoints
{
    public static void MapAuthenticationEndpoints(this IEndpointRouteBuilder app)
    {
        var endpoint = app
            .MapGroup("/authentication")
            .WithTags("Authentication");

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

        endpoint
            .MapPut("/profile", UpdateProfile)
            .Accepts<UpdateProfileRequest>("multipart/form-data")
            .Produces<NoContentResult>(StatusCodes.Status204NoContent)
            .Produces<ApplicationError>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
            .RequireAuthorization()
            .DisableAntiforgery();

        endpoint
            .MapGet("/profile", GetProfile)
            .Produces<GetProfileResponse>()
            .Produces<ApplicationError>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
            .RequireAuthorization();
    }

    private static async Task<IResult> Register([FromBody] RegisterRequest request,
        IAuthenticationService authenticationService)
    {
        var registerResult = await authenticationService.RegisterAsync(request.Adapt<RegisterBody>());
        return registerResult.Match(Results.NoContent, Results.BadRequest);
    }

    private static async Task<IResult> Login([FromBody] LoginRequest request,
        IAuthenticationService authenticationService)
    {
        var loginResult = await authenticationService.LoginAsync(request.Adapt<LoginBody>());
        return loginResult.Match(res => Results.Ok(res.Adapt<LoginResponse>()),
            Results.BadRequest);
    }

    private static async Task<IResult> UpdateProfile([FromForm] UpdateProfileRequest request,
        IAuthenticationService authenticationService)
    {
        var updateResult = await authenticationService.UpdateProfileAsync(new UpdateProfileBody(request.Name, request.Surname, request.Email, request.Password, request.Avatar));
        return updateResult.Match(Results.NoContent, Results.BadRequest);
    }

    private static async Task<IResult> GetProfile(IAuthenticationService authenticationService)
    {
        var profileResult = await authenticationService.GetProfileAsync();
        return profileResult.Match(
            result => Results.Ok(result.Profile.Adapt<GetProfileResponse>()),
            error => Results.BadRequest(error));
    }
}