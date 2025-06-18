using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiAppApi.Data;
using MiAppApi.Models;

namespace MiAppApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ViajesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ViajesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var viajes = await _context.Trips.ToListAsync();
            return Ok(viajes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var viaje = await _context.Trips.FindAsync(id);

            if (viaje == null)
            {
                return NotFound(new { mensaje = "Viaje no encontrado" });
            }

            return Ok(viaje);
        }

    }
}
