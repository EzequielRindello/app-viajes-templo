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
                    .AsNoTracking()
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

        [HttpGet("{id:int}")]  // Especifica que id debe ser entero
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                _logger.LogInformation("Obteniendo viaje con ID: {Id}", id);

                // Validar que el ID sea válido
                if (id <= 0)
                {
                    _logger.LogWarning("ID inválido recibido: {Id}", id);
                    return BadRequest(new { mensaje = "ID debe ser un número positivo" });
                }

                var viaje = await _context.Trips
                    .AsNoTracking()
                    .FirstOrDefaultAsync(v => v.Id == id);

                if (viaje == null)
                {
                    _logger.LogWarning("Viaje con ID {Id} no encontrado", id);
                    return NotFound(new { mensaje = $"Viaje con ID {id} no encontrado" });
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

        [HttpGet("{id:int}/participantes")]  // Especifica que id debe ser entero
        public async Task<IActionResult> GetParticipantesByViajeId(int id)
        {
            try
            {
                _logger.LogInformation("Obteniendo participantes del viaje con ID: {Id}", id);

                // Validar que el ID sea válido
                if (id <= 0)
                {
                    _logger.LogWarning("ID inválido recibido: {Id}", id);
                    return BadRequest(new { mensaje = "ID debe ser un número positivo" });
                }

                // Verificar que el viaje existe
                var viajeExists = await _context.Trips
                    .AsNoTracking()
                    .AnyAsync(v => v.Id == id);

                if (!viajeExists)
                {
                    _logger.LogWarning("Viaje con ID {Id} no encontrado", id);
                    return NotFound(new { mensaje = $"Viaje con ID {id} no encontrado" });
                }

                var participantes = await _context.Participants
                    .AsNoTracking()
                    .Where(p => p.TripId == id)
                    .OrderBy(p => p.Name)
                    .ToListAsync();

                _logger.LogInformation("Se encontraron {Count} participantes para el viaje con ID: {Id}",
                    participantes.Count, id);

                return Ok(participantes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener participantes del viaje con ID: {Id}", id);
                return StatusCode(500, new
                {
                    mensaje = "Error interno del servidor",
                    error = ex.Message
                });
            }
        }

        // Endpoint adicional para debugging
        [HttpGet("test")]
        public IActionResult Test()
        {
            return Ok(new
            {
                mensaje = "API funcionando correctamente",
                timestamp = DateTime.UtcNow
            });
        }
    }
}