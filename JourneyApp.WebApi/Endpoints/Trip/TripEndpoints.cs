using CSharpFunctionalExtensions;
using CSharpFunctionalExtensions.ValueTasks;
using JourneyApp.Application.Services.TripService;
using JourneyApp.Application.Services.TripService.Dto;
using JourneyApp.Core.CommonTypes;
using JourneyApp.WebApi.Authentication;
using JourneyApp.WebApi.Dto.Trip;
using JourneyApp.WebApi.Endpoints.Trip.Dto;
using MapsterMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace JourneyApp.WebApi.Endpoints.Trip;

public static class TripEndpoints
{
    public static void MapTripEndpoints(this IEndpointRouteBuilder app)
    {
        var endpoint = app.MapGroup("/trip")
            .WithTags("Trip endpoints");

        endpoint
            .MapGet("/", GetTrips)
            .Produces<List<Core.Models.Trip.Trip>>()
            .Produces<ApplicationError>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        endpoint
            .MapPost("/review/{tripId:guid}", AddTripReview)
            .Accepts<AddTripReviewRequest>("application/json")
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ApplicationError>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);
    }

    private static async Task<IResult> GetTrips([AsParameters] GetTripsRequest request, TripService tripService, IMapper mapper)
    {
        var result = await tripService.GetTripsAsync(mapper.Map<GetTripsBody>(request));
        return result.Match(v => Results.Ok(mapper.Map<List<TripDto>>(v)), Results.BadRequest);
    }

    private static async Task<IResult> AddTripReview(Guid tripId,
        [FromBody] AddTripReviewRequest request,
        TripService tripService, IMapper mapper)
    {
        var result =
            await tripService.AddTripReviewAsync(new AddTripReviewBody(tripId, request.Rating, request.Comment));
        return result.Match(Results.NoContent, Results.BadRequest);
    }
}

