using EX.Web.Consts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EX.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            // Simple validation - In production, validate against database
            if (string.IsNullOrEmpty(request.Username))
            {
                return BadRequest(new { message = "Username is required" });
            }

            // Generate JWT token with no expiration
            var token = GenerateJwtToken(request.Username, request.Role ?? "user");

            return Ok(new
            {
                token = token,
                username = request.Username,
                role = request.Role ?? "user",
                message = "Token generated successfully with no expiration"
            });
        }

        [Authorize]
        [HttpGet("validate")]
        public IActionResult Validate()
        {
            var username = User.FindFirstValue(AuthConst.UserClaim);
            var role = User.FindFirstValue(ClaimTypes.Role);

            return Ok(new
            {
                authenticated = true,
                username = username,
                role = role,
                message = "Token is valid"
            });
        }

        private string GenerateJwtToken(string username, string role)
        {
            var jwtSecretKey = _configuration["Jwt:SecretKey"];
            var jwtIssuer = _configuration["Jwt:Issuer"];
            var jwtAudience = _configuration["Jwt:Audience"];

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(AuthConst.UserClaim, username),
                new Claim(ClaimTypes.Role, role),
                new Claim(ClaimTypes.Name, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            // Create token with no expiration by not setting the expires parameter
            var token = new JwtSecurityToken(
                issuer: jwtIssuer,
                audience: jwtAudience,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: null, // No expiration
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
