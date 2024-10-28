using JourneyApp.Core.Models.User;
using JourneyApp.Core.ValueObjects.User;

namespace JourneyApp.WebApi.Endpoints.Authentication.Dto;

public record LoginResponse(string Token, UserRole Role);