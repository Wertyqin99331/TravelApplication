namespace JourneyApp.Models.TripRequest;

public class TripRequest
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public global::User User { get; set; }
    public int TripId { get; set; }
    public global::Trip Trip { get; set; }
    public TripRequestStatus Status { get; set; }

    protected TripRequest() { }

    public TripRequest(int id, int userId, int tripId, TripRequestStatus status)
    {
        this.Id = id;
        this.UserId = userId;
        this.TripId = tripId;
        this.Status = status;
    }
}

public enum TripRequestStatus
{
    Pending,
    Approved,
    Rejected
}