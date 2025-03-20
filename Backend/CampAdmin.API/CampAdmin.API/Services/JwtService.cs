using System.Security.Claims;
using System.Text;
using CampAdmin.API.Models;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace CampAdmin.API.Services
{
    public class JwtService
    {
        private readonly IConfiguration _config;

        public JwtService(IConfiguration config)
        {
            _config = config;
        }

        public string GenerateToken(User user)
        {
            // Claims
            var claims = new List<Claim>
            {
                new (JwtRegisteredClaimNames.Name, user.Username),
                new (JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            };
            foreach (var role in user.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.Trim()));
            }

            // Creds
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // gen Token Desc
            var tokenDesc = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = credentials,
                Issuer = _config["Jwt:Issuer"],
                Audience = _config["Jwt:Audience"]
            };

            // Gen Token
            var handler = new JsonWebTokenHandler();
            string token = handler.CreateToken(tokenDesc);

            return token;
        }
    }

}
