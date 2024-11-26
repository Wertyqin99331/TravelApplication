namespace JourneyApp.Application.Services.TripService.Dto;

public record AddTripReviewBody(Guid TripId, int Rating, string Comment);