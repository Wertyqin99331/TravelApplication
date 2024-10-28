using CSharpFunctionalExtensions;
using JourneyApp.Core.CommonTypes;
using Mapster;

namespace JourneyApp.Core.ValueObjects.TripReview;

public class ReviewText: ValueObject
{
    public const int MAX_REVIEW_TEXT_LENGTH = 1000;
    
    public string Value { get; } = null!;

    private ReviewText() {}

    private ReviewText(string value)
    {
        this.Value = value;
    }
    
    public static Result<ReviewText, ApplicationError> Create(string value)
    {
        if (string.IsNullOrEmpty(value))
            return new ApplicationError("Отзыв не может быть пустым");
        
        if (value.Length > MAX_REVIEW_TEXT_LENGTH)
            return new ApplicationError($"Отзыв должен быть меньше {MAX_REVIEW_TEXT_LENGTH} символов");
        
        return new ReviewText(value);
    }
    
    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return this.Value;
    }
}

public class ReviewTextMapping: IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<ReviewText, string>()
            .MapWith( src => src.Value);
    }
}