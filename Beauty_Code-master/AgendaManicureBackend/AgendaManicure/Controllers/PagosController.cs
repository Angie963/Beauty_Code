using AgendaManicure.Models;
using AgendaManicure.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

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
            if (!ObjectId.TryParse(id, out _))
                return BadRequest(new { message = "ID inválido" });

            var pago = await _pagoService.GetByIdAsync(id);
            if (pago == null)
                return NotFound(new { message = "Pago no encontrado" });

            return Ok(pago);
        }

        // POST api/pagos
[HttpPost]
public async Task<IActionResult> Create([FromBody] Pago pago)
{
    if (pago == null)
        return BadRequest(new { message = "Cuerpo inválido" });

    // validar usuario
    var usuario = await _userService.GetByIdAsync(pago.UsuarioId);
    if (usuario == null)
        return BadRequest(new { message = "Usuario referencia no existe (usuario_id inválido)" });

    // si deseas validar servicio (opcional)
    if (!string.IsNullOrWhiteSpace(pago.ServicioId))
    {
        var servicio = await _servicioService.GetByIdAsync(pago.ServicioId);
        if (servicio == null)
            return BadRequest(new { message = "Servicio referencia no existe (servicio_id inválido)" });
    }

    // guardar
    await _pagoService.CreateAsync(pago);

    return CreatedAtAction(nameof(GetById), new { id = pago.Id }, pago);
}

        
    }
}
