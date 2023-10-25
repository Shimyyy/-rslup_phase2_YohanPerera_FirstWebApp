using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using FirstWebApp.Models;

namespace FirstWebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private static readonly List<User> Users = new List<User>
        {
            new User { Id = 1, Username = "yohan", Password = "1234", Role = "Admin" },
        };

        // ... (other actions)

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            var user = Users.FirstOrDefault(u => u.Username == request.Username && u.Password == request.Password);
            if (user == null)
            {
                return Unauthorized();
            }
            var token = GenerateJwtToken(user);
            return Ok(new { user.Id, user.Username, user.Role, token });
        }

        [HttpPost("forgot-password")]
        public IActionResult ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            // In a real application, you would verify that the user exists, generate a password reset token, and send an email to the user with a link to reset their password.
            // For now, we'll just log a message and return a dummy link.
            var resetLink = "https://yourapp.com/reset-password?token=dummytoken";
            Console.WriteLine($"Password reset link for {request.Email}: {resetLink}");
            return Ok(new { Message = "Password reset link sent", ResetLink = resetLink });
        }

        private string GenerateJwtToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("your-secret-key-here"));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username ?? string.Empty),
                new Claim(ClaimTypes.Role, user.Role ?? string.Empty),
            };

            var token = new JwtSecurityToken(
                issuer: "your-issuer",
                audience: "your-audience",
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

    public class ForgotPasswordRequest
    {
        public string? Email { get; set; }
    }
}
