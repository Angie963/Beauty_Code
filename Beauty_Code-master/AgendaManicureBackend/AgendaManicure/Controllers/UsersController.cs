using AgendaManicure.Models;
using AgendaManicure.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AgendaManicure.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UserService _userService;

        public UsersController(UserService userService)
        {
            _userService = userService;
        }

        // GET api/users
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAllAsync();
            return Ok(users);
        }

        // GET api/users/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var user = await _userService.GetByIdAsync(id);

            if (user == null)
                return NotFound(new { message = "Usuario no encontrado" });

            return Ok(user);
        }

        // POST api/users
        [HttpPost]
        public async Task<IActionResult> Create(User user)
        {
            await _userService.CreateAsync(user);
            return Ok(new { message = "Usuario creado correctamente" });
        }

        // PUT api/users/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, User user)
        {
            var existing = await _userService.GetByIdAsync(id);

            if (existing == null)
                return NotFound(new { message = "Usuario no existe" });

            user.Id = id; // importante
            await _userService.UpdateAsync(id, user);

            return Ok(new { message = "Usuario actualizado" });
        }

        // DELETE api/users/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var existing = await _userService.GetByIdAsync(id);

            if (existing == null)
                return NotFound(new { message = "Usuario no existe" });

            await _userService.DeleteAsync(id);

            return Ok(new { message = "Usuario eliminado" });
        }

        // POST api/users/login
        // Acepta tanto { email, password } como { email, contrasena }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.Email))
                return BadRequest(new { message = "Email es requerido" });

            var user = await _userService.GetByEmailAsync(request.Email);
            if (user == null)
                return Unauthorized(new { message = "Email no registrado" });

            // Permitir que el frontend envíe "password" o "contrasena"
            var plain = !string.IsNullOrEmpty(request.Contrasena) ? request.Contrasena : request.Password;

            if (string.IsNullOrEmpty(plain))
                return BadRequest(new { message = "Contraseña es requerida" });

            bool valid = BCrypt.Net.BCrypt.Verify(plain, user.Contrasena);
            if (!valid)
                return Unauthorized(new { message = "Contraseña incorrecta" });

            // Respuesta: NO devolver datos sensibles
            return Ok(new
            {
                message = "Inicio de sesión exitoso",
                user = new
                {
                    id = user.Id,
                    nombre = user.Nombre,
                    email = user.Email,
                    rol = user.Roles
                }
            });
        }
    }

    // DTO de login (lo puedes mover a Models/LoginRequest.cs si prefieres)
    public class LoginRequest
    {
        public string Email { get; set; }

        // para compatibilidad con frontends que manden "password" o "contrasena"
        public string Password { get; set; }
        public string Contrasena { get; set; }
    }
}
