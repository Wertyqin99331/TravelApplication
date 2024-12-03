namespace JourneyApp.WebApi.Endpoints.Authentication.Dto;

public record GetProfileResponse(
    string Email,
    string Name,
    string Surname,
    string? AvatarUrl
);
