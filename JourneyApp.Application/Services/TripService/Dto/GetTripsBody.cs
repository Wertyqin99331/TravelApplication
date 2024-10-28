namespace JourneyApp.Application.Services.TripService.Dto;

public record GetTripsBody(int Page, int PageSize, string? City, 
    int? MinPrice, int? MaxPrice, DateOnly? StartDate, DateOnly? EndDate, 
    int? MinDaysCount, int? MaxDaysCount, int? MinRating);