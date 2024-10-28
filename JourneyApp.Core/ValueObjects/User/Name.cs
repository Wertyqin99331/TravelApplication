using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using CSharpFunctionalExtensions;
using JourneyApp.Core.CommonTypes;
using Mapster;

namespace JourneyApp.Core.ValueObjects.User;

public class Name : ValueObject
{
    public const int MAX_NAME_LENGTH = 50;

    public string Value { get; } = null!;
    

    private Name()
    {
    }

    private Name(string value)
    {
        this.Value = value;
    }

    public static Result<Name, ApplicationError> Create(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return new ApplicationError("Имя не может быть пустым");

        if (name.Length > MAX_NAME_LENGTH)
            return new ApplicationError($"Имя не может быть больше {MAX_NAME_LENGTH} символов");

        return new Name(name);
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return this.Value;
    }
}


public class NameMapping: IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Name, string>()
            .MapWith(src => src.Value);
    }
}