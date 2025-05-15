using ApiVault.DTOs;
using ApiVault.Interfaces;
using ApiVault.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiVault.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public UsuariosController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUsuarios()
        {
            var listaUsuarios = await _usuarioService.GetUsuariosAsync();
            return Ok(listaUsuarios);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("{usuarioId:int}")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUsuario(int usuarioId)
        {
            var usuario = await _usuarioService.GetUsuarioAsync(usuarioId);
            if (usuario == null)
            {
                return NotFound();
            }
            return Ok(usuario);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{usuarioId:int}")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> EliminarUsuario(int usuarioId)
        {
            var resultado = await _usuarioService.EliminarAsync(usuarioId);
            if (resultado == null)
            {
                return NotFound();
            }
            if (resultado == false)
            {
                return BadRequest("No se puede eliminar el único administrador.");
            }
            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("actualizarRol/{usuarioId:int}")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ActualizarRolUsuario(int usuarioId, [FromBody] bool esAdmin)
        {
            var resultado = await _usuarioService.ActualizarRolAsync(usuarioId, esAdmin);
            if (resultado == null)
            {
                return NotFound();
            }
            if (resultado == false)
            {
                return BadRequest("No se puede quitar el rol de administrador al único administrador.");
            }
            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{usuarioId:int}")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ActualizarUsuario(int usuarioId, [FromBody] UsuarioDto usuario)
        {
            var resultado = await _usuarioService.ActualizarAsync(usuarioId, usuario);
            if (!resultado)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}