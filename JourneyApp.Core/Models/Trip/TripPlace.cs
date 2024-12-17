using System.Text.Json.Serialization;
using CSharpFunctionalExtensions;
using JourneyApp.Core.CommonTypes;
using JourneyApp.Core.ValueObjects;
using JourneyApp.Core.ValueObjects.Common;

namespace JourneyApp.Core.Models.Trip;

public class TripPlace
{
    public const int MAX_NAME_LENGTH = 50;
    public const int MAX_DESCRIPTION_LENGTH = 500;
    
    public Guid Id { get; private set; }
    public Title Title { get; set; } = null!;
    public Description Description { get; set; } = null!;
    public double Price { get; set; }
    public string? ImageUrl { get; set; }
    
    public Guid TripDayId { get; set; }
    [JsonIgnore]
    public TripDay TripDay { get;  set; } = null!;
    
    protected TripPlace() { }

    private TripPlace(Title title, Description description, double price, Guid tripDayId, TripDay tripDay)
    {
        this.Title = title;
        this.Description = description;
        this.Price = price;
        this.TripDayId = tripDayId;
        this.TripDay = tripDay;
    }

    public static Result<TripPlace, ApplicationError> Create(string name, string description, double price, Guid tripDayId,
        TripDay tripDay)
    {
        
        var titleResult = Title.Create(name);
        if (titleResult.IsFailure)
            return titleResult.Error;
        
        var descriptionResult = Description.Create(description);
        if (descriptionResult.IsFailure)
            return descriptionResult.Error;
        
        return new TripPlace(titleResult.Value, descriptionResult.Value, price, tripDayId, tripDay);
    }
}