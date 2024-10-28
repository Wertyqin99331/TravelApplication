using System.ComponentModel.DataAnnotations;

namespace JourneyApp.WebApi.Endpoints.Authentication.Dto;

public class RegisterRequest
{
    [Required] [EmailAddress] public string Email { get;  set; } = null!;
    [Required] public string Name { get;  set; } = null!;
    [Required] public string Surname { get;  set; } = null!;
    [Required] public string Password { get; set; } = null!;
}