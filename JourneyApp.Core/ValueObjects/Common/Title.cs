using CSharpFunctionalExtensions;
using JourneyApp.Core.CommonTypes;
using Mapster;

namespace JourneyApp.Core.ValueObjects.Common;

public class Title: ValueObject
{
    public const int MAX_TITLE_LENGTH = 100;

    public string Value { get; } = null!;

    private Title() {}

    private Title(string value)
    {
        this.Value = value;
    }

    public static Result<Title, ApplicationError> Create(string title)
    {
        if (string.IsNullOrEmpty(title))
            return new ApplicationError("Заголовок не может быть пустым");
        
        if (title.Length > MAX_TITLE_LENGTH)
            return new ApplicationError($"Заголовок должен быть меньше {MAX_TITLE_LENGTH} символов");
        
        return new Title(title);
    }
    
    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return this.Value;
    }
}

public class TitleMapping: IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Title, string>()
            .MapWith(src => src.Value);
    }
}