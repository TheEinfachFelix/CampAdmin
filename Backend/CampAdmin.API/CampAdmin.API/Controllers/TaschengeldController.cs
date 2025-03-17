using CampAdmin.API.Data;
using CampAdmin.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CampAdmin.API.Controllers
{
    [Authorize(Roles = "Kassenpersonal,Admin")]
    [Route("api/taschengeld")]
    [ApiController]
    public class TaschengeldController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TaschengeldController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("buchen")]
        public async Task<IActionResult> Buchen([FromBody] TaschengeldBuchung buchung)
        {
            _context.TaschengeldBuchungen.Add(buchung);
            await _context.SaveChangesAsync();
            return Ok("Buchung erfolgreich");
        }
    }

}
