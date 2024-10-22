using System.Collections.Generic;
using JourneyApp.Models.TripRequest;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Password { get; set; }

    public ICollection<TripRequest> TripRequests { get; set; } = new List<TripRequest>();
    public ICollection<Trip> Trips { get; set; } = new List<Trip>();

    protected User() { }

    public User(int id, string name, string surname, string password)
    {
        Id = id;
        Name = name;
        Surname = surname;
        Password = password;
    }
}