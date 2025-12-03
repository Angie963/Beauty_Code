using AgendaManicure.Models;
using AgendaManicure.Services;
using Microsoft.AspNetCore.Mvc;

namespace AgendaManicure.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServiciosController : ControllerBase
    {
        private readonly ServicioService _servicioService;

        public ServiciosController(ServicioService servicioService)
        {
            _servicioService = servicioService;
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
            var servicio = await _servicioService.GetByIdAsync(id);

            if (servicio == null)
                return NotFound(new { message = "Servicio no encontrado" });

            return Ok(servicio);
        }

        // POST api/servicios
        [HttpPost]
        public async Task<IActionResult> Create(Servicio servicio)
        {
            try
            {
                await _servicioService.CreateAsync(servicio);
                return Ok(new { message = "Servicio creado correctamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // PUT api/servicios/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, Servicio servicio)
        {
            var existing = await _servicioService.GetByIdAsync(id);

            if (existing == null)
                return NotFound(new { message = "Servicio no existe" });

            servicio.Id = id;
            await _servicioService.UpdateAsync(id, servicio);

            return Ok(new { message = "Servicio actualizado" });
        }

        // DELETE api/servicios/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var existing = await _servicioService.GetByIdAsync(id);

            if (existing == null)
                return NotFound(new { message = "Servicio no existe" });

            await _servicioService.DeleteAsync(id);

            return Ok(new { message = "Servicio eliminado" });
        }
    }
}

