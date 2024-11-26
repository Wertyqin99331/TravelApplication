using CSharpFunctionalExtensions;
using JourneyApp.Core.CommonTypes;
using Mapster;

namespace JourneyApp.Core.ValueObjects.Common;

public class Country: ValueObject
{
    public const int MAX_COUNTRY_LENGTH = 100;
    
    public string Value { get; } = null!;


    private Country() {}
    
    private Country(string value)
    {
        this.Value = value;
    }

    public static Result<Country, ApplicationError> Create(string name)
    {
        if (string.IsNullOrEmpty(name))
            return new ApplicationError("Страна не может быть пустой");
        
        if (name.Length > MAX_COUNTRY_LENGTH)
            return new ApplicationError($"Название страны должно быть меньше {MAX_COUNTRY_LENGTH} символов");

        return new Country(name);
    }
    
    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return this.Value;
    }
}

public class CountryMapping: IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Country, string>()
            .MapWith(src => src.Value);
    }
}