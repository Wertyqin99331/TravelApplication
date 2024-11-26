namespace JourneyApp.WebApi.Dto.Trip;

public record TripDto(Guid Id, string City, string Description, 
    double Price, DateOnly StartDate, DateOnly EndDate, double AverageRating, List<TripDayDto> Days, List<TripReviewDto> Reviews);