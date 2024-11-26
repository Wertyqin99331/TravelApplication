namespace JourneyApp.WebApi.Endpoints.Trip.Dto;

public class AddTripReviewRequest
{
    public int Rating { get; set; }
    public string Comment { get; set; } = null!;
}