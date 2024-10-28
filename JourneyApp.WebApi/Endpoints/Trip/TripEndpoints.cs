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
    private const string Tag = "Trip endpoints";
    
    public static void MapTripEndpoints(this IEndpointRouteBuilder app)
    {
        var endpoint = app.MapGroup("/trip")
            .WithTags("Trip endpoints");

        endpoint
            .MapPost("/", GetTrips)
            .Accepts<GetTripsRequest>("application/json")
            .Produces<List<Core.Models.Trip.Trip>>()
            .Produces<ApplicationError>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);
    }

    private static async Task<IResult> GetTrips([FromBody] GetTripsRequest request, [FromServices] TripService tripService, [FromServices] IMapper mapper)
    {
        var result = await tripService.GetTrips(mapper.Map<GetTripsBody>(request));
        return result.Match(Results.Ok, Results.BadRequest);
    }
}

