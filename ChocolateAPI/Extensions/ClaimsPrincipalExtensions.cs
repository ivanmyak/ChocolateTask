using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ChocolateAPI.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static Guid GetUserId(this ClaimsPrincipal user)
        {
            var sub = user.FindFirst(ClaimTypes.NameIdentifier)?.Value
                   ?? user.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

            return Guid.TryParse(sub, out var id)
                ? id
                : throw new InvalidOperationException("User ID claim not found or invalid.");
        }
    }
}
