using CSharpFunctionalExtensions;
using JourneyApp.Application.Interfaces;
using JourneyApp.Core.Models.Trip;
using JourneyApp.Core.Models.User;
using JourneyApp.Core.ValueObjects.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace JourneyApp.Infrastructure.Database;

public class JourneyAppDbContext(IConfiguration configuration)
    : IdentityDbContext<User, IdentityRole<Guid>, Guid>, IJourneyAppDbContext
{
    public DbSet<Trip> Trips { get; set; } = null!;


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .LogTo(Console.WriteLine);

        optionsBuilder.UseNpgsql(configuration.GetConnectionString("Postgres"));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(JourneyAppDbContext).Assembly);
    }
}