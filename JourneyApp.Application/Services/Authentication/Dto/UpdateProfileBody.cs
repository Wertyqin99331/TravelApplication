using Microsoft.AspNetCore.Http;

namespace JourneyApp.Application.Services.Authentication.Dto;

public record UpdateProfileBody(
    string Name,
    string Surname,
    string Email,
    string Password,
    IFormFile? Avatar);
