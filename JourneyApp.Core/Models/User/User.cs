using CSharpFunctionalExtensions;
using JourneyApp.Core.CommonTypes;
using JourneyApp.Core.ValueObjects.User;
using Microsoft.AspNetCore.Identity;

namespace JourneyApp.Core.Models.User;

public class User : IdentityUser<Guid>
{
    public Name Name { get; set; } = null!;
    public Name Surname { get; set; } = null!;
    public string? AvatarUrl { get; set; }

    public List<TripReview.TripReview> TripReviews { get; set; } = [];
    public List<Trip.Trip> FavoriteTrips { get; set; } = [];

    public static Result<User, ApplicationError> Create(string email, string name, string surname)
    {
        var nameResult = Name.Create(name);
        if (nameResult.IsFailure)
            return nameResult.Error;
        
        var surnameResult = Name.Create(surname);
        if (surnameResult.IsFailure)
            return surnameResult.Error;
        
        var user = new User
        {
            UserName = $"{email}",
            Name = nameResult.Value,
            Surname = surnameResult.Value,
            Email = email,
            SecurityStamp = Guid.NewGuid().ToString()
        };
        
        
        
        return user;
    }

}