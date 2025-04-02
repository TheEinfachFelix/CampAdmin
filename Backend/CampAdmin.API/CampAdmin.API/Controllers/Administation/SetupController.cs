namespace CampAdmin.API.Controllers.Administation
{
    using CampAdmin.API.Data;
    using CampAdmin.API.Models;
    using CampAdmin.API.Services;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    [Route("api/setup")]
    [ApiController]
    public class SetupController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly JwtService _jwtService;

        public SetupController(AppDbContext context, JwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        [HttpPost("create-admin")]
        public IActionResult CreateAdmin()
        {
            if (_context.Users.Any(u => u.Roles.Contains("Admin")))
            {
                return BadRequest("Admin-Benutzer existiert bereits!");
            }

            var adminUser = new ApiUser
            {
                UserName = "admin",
                PasswordHash = AuthController.HashPassword("Admin"),
                Roles = new List<string> { Roles.Admin },
                Description = "Admin-Benutzer"
            };
            _context.Users.Add(adminUser);
            _context.SaveChanges();

            return Ok("new { Token = apiKey.Key }");
        }
    }
}


