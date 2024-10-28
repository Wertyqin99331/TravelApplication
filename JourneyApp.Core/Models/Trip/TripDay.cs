using System.Text.Json.Serialization;
using CSharpFunctionalExtensions;
using JourneyApp.Core.CommonTypes;
using JourneyApp.Core.ValueObjects;
using JourneyApp.Core.ValueObjects.Common;

namespace JourneyApp.Core.Models.Trip;

public class TripDay
{
    public const int MAX_CITY_LENGTH = 50;
    
    public Guid Id { get; private set; }
    public int Day { get; set; }
    public City City { get; set; } = null!;
    
    public List<TripPlace> Places { get; set; } = [];
    
    public Guid TripId { get; set; }
    [JsonIgnore]
    public Trip Trip { get; set; } = null!;

    protected TripDay() { }
    
    private TripDay(int day, City city, List<TripPlace> places, Guid tripId, Trip trip)
    {
     
        this.Day = day;
        this.City = city;
        this.Places = places;
        this.TripId = tripId;
        this.Trip = trip;
    }

    public static Result<TripDay, ApplicationError> Create(int day, string city, List<TripPlace> places,
        Guid tripId, Trip trip)
    {
        var cityResult = City.Create(city);
        if (cityResult.IsFailure)
            return cityResult.Error;
        
        return new TripDay(day, cityResult.Value, places, tripId, trip);
    }
}

