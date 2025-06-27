using System.ComponentModel.DataAnnotations;

namespace ChocolateAPI.Data
{
    public class JwtSettings
    {
        [Required] public string Issuer { get; set; } = null!;
        [Required] public string Audience { get; set; } = null!;
        [Required] public string SecretKey { get; set; } = null!;
    }
}
