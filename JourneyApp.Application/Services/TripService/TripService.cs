using CSharpFunctionalExtensions;
using JourneyApp.Application.Interfaces;
using JourneyApp.Application.Services.TripService.Dto;
using JourneyApp.Core.CommonTypes;
using JourneyApp.Core.Models.Trip;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace JourneyApp.Application.Services.TripService;

public class TripService(IJourneyAppDbContext dbContext, ILogger<TripService> logger)
{
    public async Task<Result<List<Trip>, ApplicationError>> GetTrips(GetTripsBody body)
    {
        var query = dbContext
            .Trips
            .Include(t => t.Days)
            .ThenInclude(d => d.Places)
            .AsQueryable();
        
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
            // .Where(t => t.AverageRating >= body.MinRating)
            .Skip((body.Page - 1) * body.PageSize)
            .Take(body.PageSize)
            .ToList();
        
        logger.LogInformation("Found {Count} trips", filteredTrips.Count);
        logger.LogCritical("Request was finished");
        
        return filteredTrips;
    }
}