using ChocolateAPI.Data;
using ChocolateAPI.Entites;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ChocolateAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ChocolateDbContext _db;
        private readonly JwtSettings _jwt;
        public AuthController(ChocolateDbContext db, IOptions<JwtSettings> jwt)
        {
            _db = db;
            _jwt = jwt.Value;
        }


        [HttpPost("login")]
        public IActionResult Login(ChocolateAPI.Models.LoginModel dto)
        {
            var user = _db.Users.FirstOrDefault(u => u.UserName == dto.Username);
            if (user == null || !VerifyPassword(dto.Password, user))
                return Unauthorized();

            var token = GenerateJwt(user);
            return Ok(new { token });
        }

        private bool VerifyPassword(string password, User user)
        {
            // Простой пример: сравниваем хеши (если user.PasswordHash уже хранит хеш)
            using var sha256 = SHA256.Create();
            var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            var hashString = Convert.ToBase64String(hashBytes);

            return hashString == user.PasswordHash;
        }

        private string GenerateJwt(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Role, user.Role.ToString())
    };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(3),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
