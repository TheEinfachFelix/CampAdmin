
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
        public async Task<IActionResult> Register([FromBody] User user)
        {
            user.PasswordHash = HashPassword(user.PasswordHash);
            _context.Users.Add(user);
            return Ok("Registrierung erfolgreich");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] User user)
        {
            var dbUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == user.Username);
            if (dbUser == null || dbUser.PasswordHash != HashPassword(user.PasswordHash))
                return Unauthorized("Falsche Anmeldedaten");

            string token = _jwtService.GenerateToken(dbUser);
            return Ok(new { Token = token });
        }

        [HttpPost("generate-api-key")]
        public IActionResult GenerateApiKey([FromBody] ApiKeyRequest request)
        {
            var apiKey = _apiKeyService.GenerateApiKey(request.Name, request.Description, request.Permissions);
            return Ok(new { apiKey.Key, apiKey.Permissions });
        }

        public class ApiKeyRequest
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public List<string> Permissions { get; set; }
        }

        public string Key { get; set; } = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
        public List<string> Permissions { get; set; } = new List<string>();
        public string Description { get; set; } = "";
        public string Name { get; set; } = "";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            return Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(password)));
        }
    }

}
