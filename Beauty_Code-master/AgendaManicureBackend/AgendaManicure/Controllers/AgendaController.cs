using AgendaManicure.Models;
using AgendaManicure.Services;
using Microsoft.AspNetCore.Mvc;

namespace AgendaManicure.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AgendaController : ControllerBase
    {
        private readonly AgendaService _agendaService;

        public AgendaController(AgendaService agendaService)
        {
            _agendaService = agendaService;
        }

        // GET: api/agenda
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _agendaService.GetAllAsync();
            return Ok(items);
        }

        // GET: api/agenda/usuario/{usuarioId}
        [HttpGet("usuario/{usuarioId}")]
        public async Task<IActionResult> GetByUsuario(string usuarioId)
        {
            var lista = await _agendaService.GetByUsuarioIdAsync(usuarioId);
            return Ok(lista);
        }

        // GET: api/agenda/{id}
        [HttpGet("{id}", Name = "GetAgendaById")]
        public async Task<IActionResult> GetById(string id)
        {
            var item = await _agendaService.GetByIdAsync(id);
            if (item == null) return NotFound(new { message = "Cita no encontrada" });
            return Ok(item);
        }

        // POST: api/agenda
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Cita cita)
        {
            // validaciones básicas
            if (cita == null)
                return BadRequest(new { message = "Cuerpo inválido" });

            if (cita.Fecha == default || string.IsNullOrWhiteSpace(cita.Hora) || string.IsNullOrWhiteSpace(cita.UsuarioId))
                return BadRequest(new { message = "fecha, hora y usuario_id son obligatorios" });

            // insert
            await _agendaService.CreateAsync(cita);
            return CreatedAtRoute("GetAgendaById", new { id = cita.Id }, cita);
        }

        // PUT: api/agenda/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] Cita cita)
        {
            var existing = await _agendaService.GetByIdAsync(id);
            if (existing == null) return NotFound(new { message = "Cita no existe" });

            cita.Id = id; // asegurar id
            cita.CreadoEn = existing.CreadoEn; // conservar fecha de creación si quieres

            var ok = await _agendaService.UpdateAsync(id, cita);
            if (!ok) return StatusCode(500, new { message = "No se pudo actualizar la cita" });

            return NoContent();
        }

        // DELETE: api/agenda/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var existing = await _agendaService.GetByIdAsync(id);
            if (existing == null) return NotFound(new { message = "Cita no existe" });

            var ok = await _agendaService.DeleteAsync(id);
            if (!ok) return StatusCode(500, new { message = "No se pudo eliminar la cita" });

            return NoContent();
        }
    }
}