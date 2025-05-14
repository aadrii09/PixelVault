using ApiVault.DTOs;
using ApiVault.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ApiVault.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarritosController : ControllerBase
    {
        private readonly ICarritoService _carritoService;

        public CarritosController(ICarritoService carritoService)
        {
            _carritoService = carritoService;
        }

        [HttpGet("{usuarioId}")]
        public async Task<IActionResult> GetCarritoByUsuario(int usuarioId)
        {
            if (GetUserId() != usuarioId && !IsAdmin())
            {
                return Forbid();
            }
            var carrito = await _carritoService.GetCarritoByUsuarioAsync(usuarioId);
            if (carrito == null)
            {
                return NotFound("Carrito no encontrado");
            }
            return Ok(carrito);
        }

        [HttpPost("{usuarioId}")]
        public async Task<IActionResult> AddProducto(int usuarioId, CarritoProductoDto productoDto)
        {
            if (GetUserId() != usuarioId && !IsAdmin())
            {
                return Forbid();
            }
            var result = await _carritoService.AddProductoAsync(usuarioId, productoDto);
            return Ok(result);
        }

        [HttpDelete("{usuarioId}/{productoId}")]
        public async Task<IActionResult> RemoveProducto(int usuarioId, int productoId)
        {
            if (GetUserId() != usuarioId && !IsAdmin())
            {
                return Forbid();
            }
            var result = await _carritoService.RemoveProductoAsync(usuarioId, productoId);
            return result ? NoContent() : Ok();
        }
        [HttpDelete("{usuarioId}/vaciar")]
        public async Task<IActionResult> VaciarCarrito(int usuarioId)
        {
            if (GetUserId() != usuarioId && !IsAdmin())
            {
                return Forbid();
            }
            var result = await _carritoService.ClearCarritoAsync(usuarioId);
            return result ? NoContent() : Ok();
        }

        private int GetUserId()
        {
            return int.Parse(User.FindFirst("sub")?.Value ?? "0");
        }

        private bool IsAdmin()
        {
            return User.IsInRole("Admin") ||
                   User.HasClaim(c => c.Type == "esAdmin" && c.Value == "True");
        }

    }
}