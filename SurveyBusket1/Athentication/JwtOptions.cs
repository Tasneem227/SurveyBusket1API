using System.ComponentModel.DataAnnotations;

namespace SurveyBusket1.Athentication;

public class JwtOptions
{
    public static string SectionName => "Jwt";
    [Required]
    public string key { get; init; } = string.Empty;
    [Required]
    public string Issuer { get; init; } = string.Empty;
    [Required]
    public string Audience { get; init; } = string.Empty;
    [Required]
    [Range(1, int.MaxValue)]    
    public int ExpiryMinutes { get; init; }
}
