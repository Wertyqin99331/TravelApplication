namespace JourneyApp.WebApi.Dto.Trip;

public record TripReviewDto(Guid Id, int Rating, string ReviewText, DateOnly Date);