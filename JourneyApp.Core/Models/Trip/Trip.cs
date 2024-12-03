using CSharpFunctionalExtensions;
using JourneyApp.Core.CommonTypes;
using JourneyApp.Core.ValueObjects.Common;

namespace JourneyApp.Core.Models.Trip;

public class Trip
{
    public const int MAX_CITY_LENGTH = 50;
    public const int MAX_DESCRIPTION_LENGTH = 500;

    public Guid Id { get; private set; }
    public City City { get; set; } = null!;
    public Country Country { get; set; } = null!;
    public Description Description { get; set; } = null!;
    public double Price { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }

    public double AverageRating => this.Reviews.Count == 0 ? 0 : this.Reviews.Average(r => r.Rating.Value);
    public double OverallPrice => this.Days.Sum(d => d.Places.Sum(p => p.Price));

    public List<TripDay> Days { get; set; } = [];
    public List<TripReview.TripReview> Reviews { get; set; } = [];
    public List<User.User> FavoritedByUsers { get; set; } = [];

    protected Trip()
    {
    }

    private Trip(City city, Description description, double price, DateOnly startDate, DateOnly endDate)
    {
        this.City = city;
        this.Description = description;
        this.Price = price;
        this.StartDate = startDate;
        this.EndDate = endDate;
    }

    public static Result<Trip, ApplicationError> Create(string city, string description, double price,
        DateOnly startDate, DateOnly endDate)
    {
        var cityResult = City.Create(city);
        if (cityResult.IsFailure)
            return cityResult.Error;
        
        var descriptionResult = Description.Create(description);
        if (descriptionResult.IsFailure)
            return descriptionResult.Error;
        
        return new Trip(cityResult.Value, descriptionResult.Value, price, startDate, endDate);
    }
}