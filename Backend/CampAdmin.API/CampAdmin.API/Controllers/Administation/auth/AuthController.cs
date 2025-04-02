
namespace CampAdmin.API.Controllers
{
    using CampAdmin.API.Data;
    using CampAdmin.API.Models;
    using CampAdmin.API.Services;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using System.Security.Cryptography;
    using System.Text;

    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly JwtService _jwtService;

        public AuthController(AppDbContext context, JwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUser user)
        {
            var dbUser = await _context.Users.FirstOrDefaultAsync(u => u.UserName == user.Username);
            if (dbUser == null || dbUser.PasswordHash != HashPassword(user.PasswordHash))
                return Unauthorized("Falsche Anmeldedaten");

            
            return Ok(new { Token = _jwtService.GenerateToken(dbUser) });
        }
        public class  LoginUser
        {
            public string Username { get; set; } = string.Empty;
            public string PasswordHash { get; set; } = string.Empty;
        }


        [HttpGet("tokenInfo")]
        [AllowAnonymous]
        public IActionResult getTokenInfo()
        {
            var identity = HttpContext.User.Identity;
            var isAuthenticated = identity?.IsAuthenticated ?? false;
            var claims = HttpContext.User.Claims.Select(c => new { c.Type, c.Value }).ToList();

            return Ok(new { isAuthenticated, claims });
        }

        public static string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            return Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(password)));
        }
    }
}
