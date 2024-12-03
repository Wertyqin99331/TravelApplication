namespace JourneyApp.Application.CommonTypes;

public record UserProfile(
    string Email,
    string Name,
    string Surname,
    string? AvatarUrl
);
