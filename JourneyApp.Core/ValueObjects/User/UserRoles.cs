using System.Text.Json.Serialization;

namespace JourneyApp.Core.ValueObjects.User;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum UserRole
{
    User,
    Admin
}