using CSharpFunctionalExtensions;
using JourneyApp.Core.CommonTypes;
using Mapster;

namespace JourneyApp.Core.ValueObjects.Common;

public class City: ValueObject
{
    public const int MAX_CITY_LENGTH = 100;
    
    public string Value { get; } = null!;


    private City() {}
    
    private City(string value)
    {
        this.Value = value;
    }

    public static Result<City, ApplicationError> Create(string name)
    {
        if (string.IsNullOrEmpty(name))
            return new ApplicationError("Город не может быть пустым");
        
        if (name.Length > MAX_CITY_LENGTH)
            return new ApplicationError($"Название города должно быть меньше {MAX_CITY_LENGTH} символов");

        return new City(name);
    }
    
    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return this.Value;
    }
}

public class CityMapping: IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<City, string>()
            .MapWith(src => src.Value);
    }
} 