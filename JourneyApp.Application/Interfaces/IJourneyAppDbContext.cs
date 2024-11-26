using JourneyApp.Core.Models.Trip;
using JourneyApp.Core.Models.TripReview;
using JourneyApp.Core.Models.User;
using Microsoft.EntityFrameworkCore;

namespace JourneyApp.Application.Interfaces;

public interface IJourneyAppDbContext
{
    DbSet<User> Users { get; }
    DbSet<Trip> Trips { get; }
    
    Task<int> SaveChangesAsync();
}