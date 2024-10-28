using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;

namespace JourneyApp.WebApi.Endpoints.Trip.Dto;

public class GetTripsRequest
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? City { get; set; }
    public int? MinPrice { get; set; }
    public int? MaxPrice { get; set; }
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public int? MinDaysCount { get; set; }
    public int? MaxDaysCount { get; set; }
    public int? MinRating { get; set; }
}