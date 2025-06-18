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
        private readonly ILogger<ViajesController> _logger;

        public ViajesController(ApplicationDbContext context, ILogger<ViajesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                _logger.LogInformation("Obteniendo lista de viajes");

                var viajes = await _context.Trips
                    .AsNoTracking() // Mejor rendimiento para solo lectura
                    .OrderBy(v => v.DepartureDate)
                    .ToListAsync();

                _logger.LogInformation("Se obtuvieron {Count} viajes", viajes.Count);
                return Ok(viajes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener viajes");
                return StatusCode(500, new
                {
                    mensaje = "Error interno del servidor",
                    error = ex.Message
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                _logger.LogInformation("Obteniendo viaje con ID: {Id}", id);

                var viaje = await _context.Trips
                    .AsNoTracking()
                    .FirstOrDefaultAsync(v => v.Id == id);

                if (viaje == null)
                {
                    _logger.LogWarning("Viaje con ID {Id} no encontrado", id);
                    return NotFound(new { mensaje = "Viaje no encontrado" });
                }

                _logger.LogInformation("Viaje con ID {Id} encontrado", id);
                return Ok(viaje);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener viaje con ID: {Id}", id);
                return StatusCode(500, new
                {
                    mensaje = "Error interno del servidor",
                    error = ex.Message
                });
            }
        }

    }
}