using CSharpFunctionalExtensions;
using CSharpFunctionalExtensions.ValueTasks;
using JourneyApp.Application.Services.TripService;
using JourneyApp.Application.Services.TripService.Dto;
using JourneyApp.Core.CommonTypes;
using JourneyApp.WebApi.Authentication;
using JourneyApp.Application.Dto.Trip;
using JourneyApp.WebApi.Endpoints.Trip.Dto;
using MapsterMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using IResult = Microsoft.AspNetCore.Http.IResult;
using Mapster;

namespace JourneyApp.WebApi.Endpoints.Trip;

public static class TripEndpoints
{
    public static void MapTripEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("trips")
            .WithTags("Trip");

        group.MapGet("", GetTrips)
            .WithName("GetTrips")
            .WithOpenApi()
            .Produces<List<TripDto>>()
            .Produces<ApplicationError>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        group.MapGet("favorites", GetFavoriteTrips)
            .WithName("GetFavoriteTrips")
            .RequireAuthorization()
            .WithOpenApi()
            .Produces<List<TripDto>>()
            .Produces<ApplicationError>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        group.MapPost("{tripId:guid}/reviews", AddTripReview)
            .WithName("AddTripReview")
            .WithOpenApi()
            .Accepts<AddTripReviewRequest>("application/json")
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ApplicationError>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        group.MapPost("{tripId:guid}/favorites", AddToFavorites)
            .WithName("AddToFavorites")
            .WithOpenApi()
            .RequireAuthorization()
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ApplicationError>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        group.MapDelete("favorites", RemoveFromFavorites)
            .WithName("RemoveFromFavorites")
            .WithOpenApi()
            .RequireAuthorization()
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ApplicationError>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);
    }

    private static async Task<IResult> GetTrips([AsParameters] GetTripsRequest request, TripService tripService)
    {
        var result = await tripService.GetTripsAsync(request.Adapt<GetTripsBody>());
        return result.Match(Results.Ok, Results.BadRequest);
    }

    private static async Task<IResult> AddTripReview(Guid tripId,
        [FromBody] AddTripReviewRequest request,
        TripService tripService)
    {
        var result =
            await tripService.AddTripReviewAsync(new AddTripReviewBody(tripId, request.Rating, request.Comment));
        return result.Match(Results.NoContent, Results.BadRequest);
    }

    private static async Task<IResult> GetFavoriteTrips(TripService tripService)
    {
        var result = await tripService.GetFavoriteTripsAsync();
        return result.Match(Results.Ok, Results.BadRequest);
    }

    private static async Task<IResult> AddToFavorites(Guid tripId, TripService tripService)
    {
        var result = await tripService.AddToFavoritesAsync(tripId);
        return result.Match(Results.NoContent, Results.BadRequest);
    }

    private static async Task<IResult> RemoveFromFavorites([FromBody] List<Guid> tripIds, TripService tripService)
    {
        var result = await tripService.RemoveFromFavoritesAsync(tripIds);
        return result.Match(Results.NoContent, Results.BadRequest);
    }
}
