using CampAdmin.API.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CampAdmin.API.Controllers
{
    [Authorize(Roles = "Betreuer,Admin")]
    [Route("api/teilnehmer")]
    [ApiController]
    public class TeilnehmerController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TeilnehmerController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTeilnehmer()
        {
            return Ok(await _context.Teilnehmer.ToListAsync());
        }
    }

}
