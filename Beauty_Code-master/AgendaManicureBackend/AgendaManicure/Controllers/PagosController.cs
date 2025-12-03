using AgendaManicure.Models;
using AgendaManicure.Services;
using Microsoft.AspNetCore.Mvc;

namespace AgendaManicure.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PagosController : ControllerBase
    {
        private readonly PagoService _pagoService;
        private readonly UserService _userService;
        private readonly ServicioService _servicioService;

        public PagosController(PagoService pagoService, UserService userService, ServicioService servicioService)
        {
            _pagoService = pagoService;
            _userService = userService;
            _servicioService = servicioService;
        }

        // GET api/pagos
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _pagoService.GetAllAsync();
            return Ok(list);
        }

        // GET api/pagos/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var pago = await _pagoService.GetByIdAsync(id);
            if (pago == null) return NotFound(new { message = "Pago no encontrado" });
            return Ok(pago);
        }

        // POST api/pagos
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Pago pago)
        {
            if (pago == null)
                return BadRequest(new { message = "Cuerpo inválido" });

            // Validar usuario
            var usuario = await _userService.GetByIdAsync(pago.UsuarioId);
            if (usuario == null)
                return BadRequest(new { message = "Usuario referencia no existe (usuario_id inválido)" });

            // Si viene servicio_id, validar que exista
            if (!string.IsNullOrWhiteSpace(pago.ServicioId))
            {
                var servicio = await _servicioService.GetByIdAsync(pago.ServicioId);
                if (servicio == null)
                    return BadRequest(new { message = "Servicio referencia no existe (servicio_id inválido)" });
            }

            // normalizar/valores por defecto
            pago.CreadoEn = DateTime.UtcNow;
            await _pagoService.CreateAsync(pago);

            return CreatedAtAction(nameof(GetById), new { id = pago.Id }, pago);
        }

        // PUT api/pagos/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] Pago pago)
        {
            var existing = await _pagoService.GetByIdAsync(id);
            if (existing == null) return NotFound(new { message = "Pago no existe" });

            // Si cambia UsuarioId o ServicioId, opcionalmente validar aquí (similar al POST)
            pago.Id = id;
            await _pagoService.UpdateAsync(id, pago);
            return NoContent();
        }

        // DELETE api/pagos/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var existing = await _pagoService.GetByIdAsync(id);
            if (existing == null) return NotFound(new { message = "Pago no existe" });

            await _pagoService.DeleteAsync(id);
            return NoContent();
        }

        // GET api/pagos/usuario/{usuarioId}
        [HttpGet("usuario/{usuarioId}")]
        public async Task<IActionResult> GetByUsuario(string usuarioId)
        {
            var pagos = await _pagoService.GetByUsuarioIdAsync(usuarioId);
            return Ok(pagos);
        }
    }
}
