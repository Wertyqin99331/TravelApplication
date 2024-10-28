using System.ComponentModel.DataAnnotations;

namespace JourneyApp.WebApi.Endpoints.Authentication.Dto;

public class LoginRequest
{
    [Required] [EmailAddress] public string Email { get;  set; } = null!;
    [Required] public string Password { get;  set; } = null!;
}