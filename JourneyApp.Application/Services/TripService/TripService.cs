using CSharpFunctionalExtensions;
using JourneyApp.Application.Interfaces;
using JourneyApp.Application.Services.TripService.Dto;
using JourneyApp.Core.CommonTypes;
using JourneyApp.Core.Models.Trip;
using JourneyApp.Core.Models.TripReview;
using JourneyApp.Core.Models.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace JourneyApp.Application.Services.TripService;

public class TripService(IJourneyAppDbContext dbContext, UserService.UserService userService, ILogger<TripService> logger)
{
    public async Task<Result<List<Trip>, ApplicationError>> GetTripsAsync(GetTripsBody body)
    {
        logger.LogInformation($"Min rating is {body.MinRating}");
        
        var query = dbContext
            .Trips
            .Include(t => t.Days)
            .ThenInclude(d => d.Places)
            .Include(t => t.Reviews)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(body.Country))
        {
            query = query
                .Where(t => t.Country.Value == body.Country);
        }
        
        if (!string.IsNullOrWhiteSpace(body.City))
        {
            query = query
                .Where(t => t.City.Value == body.City);
        }
        
        if (body.MinPrice is not null)
        {
            query = query
                .Where(t => t.Price >= body.MinPrice);
        }
        
        if (body.MaxPrice is not null)
        {
            query = query
                .Where(t => t.Price <= body.MaxPrice);
        }
        
        if (body.StartDate is not null)
        {
            query = query
                .Where(t => t.StartDate >= body.StartDate);
        }

        if (body.EndDate is not null)
        {
            query = query
                .Where(t => t.EndDate <= body.EndDate);
        }
        
        if (body.MinDaysCount is not null)
        {
            query = query
                .Where(t => t.Days.Count >= body.MinDaysCount);
        }
        
        if (body.MaxDaysCount is not null)
        {
            query = query
                .Where(t => t.Days.Count <= body.MaxDaysCount);
        }

        var trips = await query.ToListAsync();

        var filteredTrips = trips
            .Skip((body.Page - 1) * body.PageSize)
            .Take(body.PageSize)
            .ToList();

        var tripsWithRating = filteredTrips
            // .Where(t => t.AverageRating >= body.MinRating)
            .ToList();
        logger.LogInformation($"Result len is {tripsWithRating.Count}");
        
        return tripsWithRating;
    }

    public async Task<UnitResult<ApplicationError>> AddTripReviewAsync(AddTripReviewBody body)
    {
        var trip = await dbContext
            .Trips
            .FirstOrDefaultAsync(t => t.Id == body.TripId);
        
        if (trip is null)
            return new ApplicationError("Путешествие не найдено");

        var userResult = await userService.GetUserFromTokenAsync();
        if (userResult.IsFailure)
            return userResult.Error;
        
        var reviewResult = TripReview.Create(body.Rating, body.Comment, DateOnly.FromDateTime(DateTime.UtcNow), userResult.Value.Id, userResult.Value, trip.Id, trip);

        if (reviewResult.IsFailure)
            return reviewResult.Error;
        
        trip.Reviews.Add(reviewResult.Value);
        await dbContext.SaveChangesAsync();
        
        return UnitResult.Success<ApplicationError>();
    }
}