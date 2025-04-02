using CampAdmin.API.Data;
using CampAdmin.API.Models;
using CampAdmin.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure.Internal;

namespace CampAdmin.API.Controllers.auth
{
    [Route("api/admin")]
    [ApiController]
    [Authorize(Roles = Roles.Admin)]
    public class AdminController:ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly JwtService _jwtService;

        public AdminController(AppDbContext context, JwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }
        [HttpGet("user")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _context.Users.ToListAsync();
            return Ok(users);
        }

        [HttpPut("user/{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] RegisterUser user)
        {
            var dbUser = await GetUserAsync(id);
            if (dbUser == null) return NotFound("User not found");

            dbUser.UserName = user.Username;
            dbUser.PasswordHash = AuthController.HashPassword(user.PasswordHash);
            dbUser.Description = user.Description;
            dbUser.Roles = user.Roles;
            await _context.SaveChangesAsync();
            return Ok("User updated");
        }

        [HttpPost("user")]
        public async Task<IActionResult> Register([FromBody] RegisterUser user)
        {
            var newUser = new ApiUser
            {
                UserName = user.Username,
                PasswordHash = AuthController.HashPassword(user.PasswordHash),
                Description = user.Description,
                Roles = user.Roles,
            };
            _context.Users.Add(newUser);

            await _context.SaveChangesAsync();
            return Ok("Added new user");
        }

        [HttpDelete("user/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await GetUserAsync(id);
            if (user == null) return NotFound("User not found");

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return Ok("User deleted");
        }
        public class RegisterUser
        {
            public string Username { get; set; } = string.Empty;
            public string PasswordHash { get; set; } = string.Empty;
            public List<string> Roles { get; set; } = [];
            public string Description { get; set; } = "";
        }

        private async Task<ApiUser?> GetUserAsync(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        }
    }
}
