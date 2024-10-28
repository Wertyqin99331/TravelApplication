using System.ComponentModel.DataAnnotations;

namespace JourneyApp.Application.Options;

public class JwtOptions
{
    public const string SECTION_NAME = "JwtTokenOptions";
    
    [Required] public string Secret { get; init; } = null!;
    [Required] public bool ValidateIssuerSigningKey { get; init; }
    
    [Required] public string Issuer { get; init; } = null!;
    [Required] public bool ValidateIssuer { get; init; }
    
    [Required] public string Audience { get; init; } = null!;
    [Required] public bool ValidateAudience { get; init; }
    
    [Required] public int DurationInHours { get; init; }
    [Required] public bool ValidateLifetime { get; init; }
}
