namespace JourneyApp.Application.Dto.Trip;

public record TripPlaceDto(
    Guid Id,
    string Title,
    string Description,
    double Price);
