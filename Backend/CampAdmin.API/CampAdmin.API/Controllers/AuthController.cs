
namespace CampAdmin.API.Controllers
{
    using CampAdmin.API.Data;
    using CampAdmin.API.Models;
    using CampAdmin.API.Services;
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
        private readonly ApiKeyService _apiKeyService;

        public AuthController(AppDbContext context, JwtService jwtService, ApiKeyService apiKeyService)
        {
            _context = context;
            _jwtService = jwtService;
            _apiKeyService = apiKeyService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUser user)
        {
            _context.Users.Add(new User
            {
                Username = user.Username,
                PasswordHash = HashPassword(user.PasswordHash),
                Role = user.Role
            });
            _context.SaveChanges();
            return Ok("Registrierung erfolgreich");
        }
        public class RegisterUser
        {
            public string Username { get; set; } = string.Empty;
            public string PasswordHash { get; set; } = string.Empty;
            public string Role { get; set; } = "";
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUser user)
        {
            var dbUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == user.Username);
            if (dbUser == null || dbUser.PasswordHash != HashPassword(user.PasswordHash))
                return Unauthorized("Falsche Anmeldedaten");

            string token = _jwtService.GenerateToken(dbUser);
            return Ok(new { Token = token });
        }
        public class  LoginUser
        {
            public string Username { get; set; } = string.Empty;
            public string PasswordHash { get; set; } = string.Empty;
        }

        [HttpPost("generate-api-key")]
        public IActionResult GenerateApiKey([FromBody] ApiKeyRequest request)
        {
            var apiKey = _apiKeyService.GenerateApiKey(request.Name, request.Description, request.Permissions);
            return Ok(new { apiKey.Key, apiKey.Permissions });
        }

        public class ApiKeyRequest
        {
            public string Name { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;
            public List<string> Permissions { get; set; } = new List<string>();
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            return Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(password)));
        }
    }

}
