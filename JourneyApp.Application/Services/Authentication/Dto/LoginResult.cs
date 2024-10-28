using JourneyApp.Core.ValueObjects.User;

namespace JourneyApp.Application.Services.Authentication.Dto;

public record LoginResult(string Token, UserRole Role);