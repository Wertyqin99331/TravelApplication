namespace JourneyApp.Application.Dto.Trip;

public record TripDayDto(
    Guid Id,
    int Day,
    string City,
    List<TripPlaceDto> Places);
