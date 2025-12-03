using AgendaManicure.Models;
using AgendaManicure.Services;
using Microsoft.AspNetCore.Mvc;

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
    }
}
