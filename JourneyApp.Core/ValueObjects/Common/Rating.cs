using CSharpFunctionalExtensions;
using JourneyApp.Core.CommonTypes;
using Mapster;

namespace JourneyApp.Core.ValueObjects.Common;

public class Rating: ValueObject
{
    public const int MIN_RATING_VALUE = 1;
    public const int MAX_RATING_VALUE = 5;
    
    public int Value { get; }
    
    private Rating(int value)
    {
        this.Value = value;
    }
    
    public static Result<Rating, ApplicationError> Create(int value)
    {
        if (value is < MIN_RATING_VALUE or > MAX_RATING_VALUE)
            return new ApplicationError($"Рейтинг должен быть в диапазоне от {MIN_RATING_VALUE} до {MAX_RATING_VALUE}");
        
        return new Rating(value);
    }
    
    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return this.Value;
    }
}

public class RatingMapping: IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Rating, int>()
            .MapWith(src => src.Value);
    }
}