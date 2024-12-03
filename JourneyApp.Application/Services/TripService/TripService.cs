using CSharpFunctionalExtensions;
using JourneyApp.Application.Interfaces;
using JourneyApp.Application.Services.TripService.Dto;
using JourneyApp.Application.Services.UserService;
using JourneyApp.Core.CommonTypes;
using JourneyApp.Core.Models.Trip;
using JourneyApp.Core.Models.TripReview;
using JourneyApp.Core.Models.User;
using JourneyApp.Application.Dto.Trip;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace JourneyApp.Application.Services.TripService;

public class TripService(IJourneyAppDbContext dbContext, IUserService userService, ILogger<TripService> logger)
{
    public async Task<Result<List<TripDto>, ApplicationError>> GetTripsAsync(GetTripsBody body)
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

        var trips = await query.ProjectToType<TripDto>()
            .ToListAsync();

        var filteredTrips = trips
            .Skip((body.Page - 1) * body.PageSize)
            .Take(body.PageSize)
            .ToList();

        var tripsWithRating = filteredTrips
            .ToList();
        logger.LogInformation($"Result len is {tripsWithRating.Count}");

        var tripDtos = tripsWithRating.Adapt<List<TripDto>>();
        return tripDtos;
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

    public async Task<UnitResult<ApplicationError>> AddToFavoritesAsync(Guid tripId)
    {
        var currentUser = await userService.GetUserFromTokenAsync();
        if (currentUser.IsFailure)
            return currentUser.Error;

        var trip = await dbContext.Trips
            .Include(t => t.FavoritedByUsers)
            .FirstOrDefaultAsync(t => t.Id == tripId);
            
        if (trip == null)
            return new ApplicationError("Trip not found");

        if (trip.FavoritedByUsers.Any(u => u.Id == currentUser.Value.Id))
            return new ApplicationError("Trip is already in favorites");

        trip.FavoritedByUsers.Add(currentUser.Value);
        await dbContext.SaveChangesAsync();

        return UnitResult.Success<ApplicationError>();
    }

    public async Task<UnitResult<ApplicationError>> RemoveFromFavoritesAsync(IEnumerable<Guid> tripIds)
    {
        var currentUser = await userService.GetUserFromTokenAsync();
        if (currentUser.IsFailure)
            return currentUser.Error;

        var tripsToRemove = await dbContext.Trips
            .Include(t => t.FavoritedByUsers)
            .Where(t => tripIds.Contains(t.Id) && t.FavoritedByUsers.Any(u => u.Id == currentUser.Value.Id))
            .ToListAsync();

        foreach (var trip in tripsToRemove)
        {
            trip.FavoritedByUsers.Remove(currentUser.Value);
        }

        await dbContext.SaveChangesAsync();
        return UnitResult.Success<ApplicationError>();
    }

    public async Task<Result<List<TripDto>, ApplicationError>> GetFavoriteTripsAsync()
    {
        var currentUser = await userService.GetUserFromTokenAsync();
        if (currentUser.IsFailure)
            return currentUser.Error;

        var favoriteTrips = await dbContext.Trips
            .Include(t => t.Days)
                .ThenInclude(d => d.Places)
            .Include(t => t.Reviews)
            .Include(t => t.FavoritedByUsers)
            .Where(t => t.FavoritedByUsers.Any(u => u.Id == currentUser.Value.Id))
            .ProjectToType<TripDto>()
            .ToListAsync();

        return favoriteTrips;
    }
}