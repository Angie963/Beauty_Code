using AgendaManicure.Models;
using AgendaManicure.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace AgendaManicure.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServiciosController : ControllerBase
    {
        private readonly ServicioService _servicioService;
        private readonly CategoriaService _categoriaService;

        public ServiciosController(ServicioService servicioService, CategoriaService categoriaService)
        {
            _servicioService = servicioService;
            _categoriaService = categoriaService;
        }

        // GET api/servicios
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var servicios = await _servicioService.GetAllAsync();
            return Ok(servicios);
        }

        // GET api/servicios/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            if (!ObjectId.TryParse(id, out _))
                return BadRequest(new { message = "ID inválido" });

            var servicio = await _servicioService.GetByIdAsync(id);

            if (servicio == null)
                return NotFound(new { message = "Servicio no encontrado" });

            return Ok(servicio);
        }

        // POST api/servicios
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Servicio servicio)
        {
            if (servicio == null)
                return BadRequest(new { message = "Datos inválidos" });

            // Validar categoría
            var categoria = await _categoriaService.GetByIdAsync(servicio.CategoriaId);
            if (categoria == null)
                return BadRequest(new { message = "La categoría no existe (categoria_id inválido)" });

            servicio.CreadoEn = DateTime.UtcNow;

            await _servicioService.CreateAsync(servicio);

            return CreatedAtAction(nameof(GetById), new { id = servicio.Id }, servicio);
        }

        // PUT api/servicios/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] Servicio servicio)
        {
            if (!ObjectId.TryParse(id, out _))
                return BadRequest(new { message = "ID inválido" });

            var existing = await _servicioService.GetByIdAsync(id);
            if (existing == null)
                return NotFound(new { message = "Servicio no existe" });

            // Validar categoría si cambió
            if (!string.IsNullOrWhiteSpace(servicio.CategoriaId))
            {
                var categoria = await _categoriaService.GetByIdAsync(servicio.CategoriaId);
                if (categoria == null)
                    return BadRequest(new { message = "La categoría no existe" });
            }

            servicio.Id = id;
            await _servicioService.UpdateAsync(id, servicio);

            return Ok(new { message = "Servicio actualizado" });
        }

        // DELETE api/servicios/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            if (!ObjectId.TryParse(id, out _))
                return BadRequest(new { message = "ID inválido" });

            var existing = await _servicioService.GetByIdAsync(id);
            if (existing == null)
                return NotFound(new { message = "Servicio no existe" });

            await _servicioService.DeleteAsync(id);

            return Ok(new { message = "Servicio eliminado" });
        }
    }
}

