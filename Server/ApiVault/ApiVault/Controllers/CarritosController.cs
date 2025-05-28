using ApiVault.DTOs;
using ApiVault.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ApiVault.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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
            if (!EsPropietario(usuarioId)) return Forbid();

            var carrito = await _carritoService.GetCarritoByUsuarioAsync(usuarioId);
            if (carrito == null) return NotFound("Carrito no encontrado");

            return Ok(carrito);
        }

        [HttpPost("{usuarioId}")]
        public async Task<IActionResult> AddProducto(int usuarioId, CarritoProductoDto productoDto)
        {
            Console.WriteLine("🧩 LISTA COMPLETA DE CLAIMS:");
            foreach (var claim in User.Claims)
            {
                Console.WriteLine($"   ▶ {claim.Type}: {claim.Value}");
            }

            var actualUserId = GetUserId();
            Console.WriteLine($"🔵 Solicitud de usuarioId {usuarioId}, autenticado como {actualUserId}");

            if (!EsPropietario(usuarioId))
            {
                Console.WriteLine("❌ Usuario no autorizado (403)");
                return Forbid();
            }

            var result = await _carritoService.AddProductoAsync(usuarioId, productoDto);

            Console.WriteLine("🔎 BODY RECIBIDO:");
            Console.WriteLine($"   ID: {productoDto.IdProducto}");
            Console.WriteLine($"   Nombre: {productoDto.Nombre}");
            Console.WriteLine($"   ImagenUrl: {productoDto.ImagenUrl}");
            Console.WriteLine($"   Cantidad: {productoDto.Cantidad}");
            Console.WriteLine($"   PrecioUnitario: {productoDto.PrecioUnitario}");

            return Ok(result);
        }


        [HttpDelete("{usuarioId}/{productoId}")]
        public async Task<IActionResult> RemoveProducto(int usuarioId, int productoId)
        {
            if (!EsPropietario(usuarioId)) return Forbid();

            var result = await _carritoService.RemoveProductoAsync(usuarioId, productoId);
            return result ? NoContent() : Ok();
        }

        [HttpDelete("{usuarioId}/vaciar")]
        public async Task<IActionResult> VaciarCarrito(int usuarioId)
        {
            if (!EsPropietario(usuarioId)) return Forbid();

            var result = await _carritoService.ClearCarritoAsync(usuarioId);
            return result ? NoContent() : Ok();
        }
        private bool EsPropietario(int usuarioId)
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (int.TryParse(claim, out var userId))
            {
                return userId == usuarioId || IsAdmin();
            }
            return false;
        }

        private int GetUserId()
        {
            var raw = User.FindFirst("sub")?.Value;
            Console.WriteLine($"🟡 Claim 'sub' (manual): {raw}");
            return int.TryParse(raw, out var id) ? id : 0;
        }

        private bool IsAdmin()
        {
            return User.IsInRole("Admin") || User.HasClaim(c => c.Type == "esAdmin" && c.Value == "True");
        }
    }
}