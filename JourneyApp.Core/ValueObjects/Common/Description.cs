using CSharpFunctionalExtensions;
using JourneyApp.Core.CommonTypes;
using Mapster;

namespace JourneyApp.Core.ValueObjects.Common;

public class Description : ValueObject
{
    public const int MAX_DESCRIPTION_LENGTH = 500;

    public string Value { get; } = null!;


    private Description() { }
    
    private Description(string value)
    {
        this.Value = value;
    }

    public static Result<Description, ApplicationError> Create(string value)
    {
        if (string.IsNullOrEmpty(value))
            return new ApplicationError("Описание не может быть пустым");
        
        if (value.Length > MAX_DESCRIPTION_LENGTH)
            return new ApplicationError($"Описание должно быть меньше {MAX_DESCRIPTION_LENGTH} символов");

        return new Description(value);
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return this.Value;
    }
}

public class DescriptionMapping: IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Description, string>()
            .MapWith(src => src.Value);
    }
}