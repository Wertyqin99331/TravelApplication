namespace JourneyApp.Application.Dto.Trip;

public record TripDto(
    Guid Id,
    string City,
    string Country,
    string Description,
    double Price,
    DateOnly StartDate,
    DateOnly EndDate,
    double AverageRating,
    string? ImageUrl,
    List<TripDayDto> Days,
    List<TripReviewDto> Reviews);
