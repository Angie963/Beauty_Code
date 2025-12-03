using AgendaManicure.Models;
using AgendaManicure.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace AgendaManicure.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ResenasController : ControllerBase
    {
        private readonly ResenasService _resenasService;

        public ResenasController(ResenasService resenasService)
        {
            _resenasService = resenasService;
        }

        // GET api/resenas
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _resenasService.GetAllAsync();
            return Ok(items);
        }

        // GET api/resenas/{id}
        [HttpGet("{id}", Name = "GetResenaById")]
        public async Task<IActionResult> GetById(string id)
        {
            var item = await _resenasService.GetByIdAsync(id);
            if (item == null) return NotFound(new { message = "Reseña no encontrada" });
            return Ok(item);
        }

        // POST api/resenas
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Resena resena)
        {
            if (resena == null) return BadRequest(new { message = "Cuerpo inválido" });

            try
            {
                await _resenasService.CreateAsync(resena);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (MongoWriteException mwx)
            {
                return BadRequest(new { message = "Error al guardar la reseña", detail = mwx.Message });
            }

            return CreatedAtRoute("GetResenaById", new { id = resena.Id }, resena);
        }

        // PUT api/resenas/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] Resena resena)
        {
            var existing = await _resenasService.GetByIdAsync(id);
            if (existing == null) return NotFound(new { message = "Reseña no existe" });

            try
            {
                resena.Id = id;
                await _resenasService.UpdateAsync(id, resena);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }

            return NoContent();
        }

        // DELETE api/resenas/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var existing = await _resenasService.GetByIdAsync(id);
            if (existing == null) return NotFound(new { message = "Reseña no existe" });

            await _resenasService.DeleteAsync(id);
            return NoContent();
        }
    }
}
