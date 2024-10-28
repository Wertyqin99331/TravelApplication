using System.ComponentModel.DataAnnotations;

namespace JourneyApp.Infrastructure.Options;

public class IdentityPasswordOptions
{
    public const string SECTION_NAME = "IdentityPasswordOptions";
    
    [Required] public bool RequireDigit { get; init; }
    [Required] public bool RequireLowercase { get; init; }
    [Required] public bool RequireUppercase { get; init; }
    [Required] public bool RequireNonAlphanumeric { get; init; }
    [Required] public int RequiredLength { get; init; }
    [Required] public bool RequireUniqueEmail { get; init; }
}
