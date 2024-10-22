using System;
using System.Collections.Generic;
using JourneyApp.Models.TripRequest;

public class Trip
{
    public int Id { get; set; }
    public string Destination { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }

    public ICollection<TripRequest> TripRequests { get; set; } = new List<TripRequest>();

    // Protected constructor for EF Core
    protected Trip() { }

    // Public constructor
    public Trip(int id, string destination, DateTime startDate, DateTime endDate, int userId)
    {
        Id = id;
        Destination = destination;
        StartDate = startDate;
        EndDate = endDate;
        UserId = userId;
    }
}