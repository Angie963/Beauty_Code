using AgendaManicure.Models;
using AgendaManicure.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using System;

namespace AgendaManicure.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriasController : ControllerBase
    {
        private readonly CategoriaService _categoriaService;

        public CategoriasController(CategoriaService categoriaService)
        {
            _categoriaService = categoriaService;
        }

        // GET: api/categorias
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var items = await _categoriaService.GetAllAsync();
                return Ok(items);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al obtener categorías", detail = ex.Message });
            }
        }

        // GET: api/categorias/{id}
        [HttpGet("{id}", Name = "GetCategoriaById")]
        public async Task<IActionResult> GetById(string id)
        {
            if (!ObjectId.TryParse(id, out _))
                return BadRequest(new { message = "Formato de ID inválido" });

            try
            {
                var item = await _categoriaService.GetByIdAsync(id);
                if (item == null)
                    return NotFound(new { message = "Categoría no encontrada" });

                return Ok(item);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al consultar la categoría", detail = ex.Message });
            }
        }

        // POST: api/categorias
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Categoria categoria)
        {
            if (categoria == null || string.IsNullOrWhiteSpace(categoria.TipoDeServicio))
                return BadRequest(new { message = "El campo tipo_de_servicio es obligatorio" });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _categoriaService.CreateAsync(categoria);
            }
            catch (MongoWriteException)
            {
                return BadRequest(new { message = "Ya existe una categoría con ese tipo_de_servicio." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al crear la categoría", detail = ex.Message });
            }

            return CreatedAtRoute("GetCategoriaById", new { id = categoria.Id }, categoria);
        }

        // PUT: api/categorias/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] Categoria categoria)
        {
            if (!ObjectId.TryParse(id, out _))
                return BadRequest(new { message = "Formato de ID inválido" });

            var existing = await _categoriaService.GetByIdAsync(id);
            if (existing == null)
                return NotFound(new { message = "La categoría no existe" });

            try
            {
                categoria.Id = id; // mantener el mismo ID
                await _categoriaService.UpdateAsync(id, categoria);
            }
            catch (MongoWriteException)
            {
                return BadRequest(new { message = "El tipo_de_servicio ya está en uso por otra categoría." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al actualizar la categoría", detail = ex.Message });
            }

            return NoContent();
        }

        // DELETE: api/categorias/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            if (!ObjectId.TryParse(id, out _))
                return BadRequest(new { message = "Formato de ID inválido" });

            var existing = await _categoriaService.GetByIdAsync(id);
            if (existing == null)
                return NotFound(new { message = "La categoría no existe" });

            try
            {
                await _categoriaService.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al eliminar la categoría", detail = ex.Message });
            }

            return NoContent();
        }
    }
}

