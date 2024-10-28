namespace JourneyApp.Application.Services.Authentication.Dto;

public record RegisterBody(string Name, string Surname, string Email, string Password);