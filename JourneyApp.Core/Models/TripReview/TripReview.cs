using CSharpFunctionalExtensions;
using JourneyApp.Core.CommonTypes;
using JourneyApp.Core.ValueObjects.Common;
using JourneyApp.Core.ValueObjects.TripReview;

namespace JourneyApp.Core.Models.TripReview;

public class TripReview
{
    public const int MAX_REVIEW_LENGTH = 1000;

    public Guid Id { get; private set; }
    public Rating Rating { get; private set; } = null!;
    public ReviewText ReviewText { get; private set; } = null!;
    public DateOnly Date { get; private set; }

    public Guid UserId { get; private set; }
    public User.User User { get; private set; } = null!;

    public Guid TripId { get; private set; }
    public Trip.Trip Trip { get; private set; } = null!;

    private TripReview()
    {
    }

    private TripReview(Rating rating, ReviewText reviewText, DateOnly date, Guid userId, User.User user,
        Guid tripId, Trip.Trip trip)
    {
        this.Rating = rating;
        this.ReviewText = reviewText;
        this.Date = date;
        this.UserId = userId;
        this.User = user;
        this.TripId = tripId;
        this.Trip = trip;
    }

    public static Result<TripReview, ApplicationError> Create(int rating, string reviewText, DateOnly date, Guid userId,
        User.User user, Guid tripId, Trip.Trip trip)
    {
        var reviewTextResult = ReviewText.Create(reviewText);
        if (reviewTextResult.IsFailure)
            return reviewTextResult.Error;
        
        var ratingResult = Rating.Create(rating);
        if (ratingResult.IsFailure)
            return ratingResult.Error;
        
        return new TripReview(ratingResult.Value, reviewTextResult.Value, date, userId, user, tripId, trip);
    }
}