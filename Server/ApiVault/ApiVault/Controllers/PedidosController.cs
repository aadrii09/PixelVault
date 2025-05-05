using ApiVault.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiVault.Controllers
{
    [Authorize] //jwt
    [Route("api/[controller]")]
    [ApiController]
    public class PedidosController : ControllerBase
    {
        private readonly IPedidoService _pedidoService;

        public PedidosController(IPedidoService pedidoService)
        {
            _pedidoService = pedidoService;
        }

        [HttpPost("{idUsuario}/crear")]
        public async Task<IActionResult> CrearPedido(int idUsuario, [FromBody] string metodoPago = "Tarjeta")
        {

            if (GetUserId() != idUsuario && !IsAdmin())
            {
                return Forbid();
            }
            var pedido = await _pedidoService.CrearPedidoDesdeCarritoAsync(idUsuario, metodoPago);
            if (pedido == null) return BadRequest("No se pudo crear el pedido o el carrito está vacío.");
            return Ok(pedido);

        }

        [HttpGet("{idUsuario}")]
        public async Task<IActionResult> GetPedidosUsuario(int idUsuario)
        {
            if (GetUserId() != idUsuario && !IsAdmin())
            {
                return Forbid();
            }
            var pedidos = await _pedidoService.GetPedidosByUsuarioAsync(idUsuario);
            return Ok(pedidos);
        }
        [Authorize(Roles = "Admin")] //jwt
        [HttpGet("todos(admin)")]
        public async Task<IActionResult> GetTodos()
        {
            var pedidos = await _pedidoService.GetTodosAsync();
            return Ok(pedidos);
        }

        [HttpPost("detalle/{id}")]
        public async Task<IActionResult> GetDetalle(int id)
        {
            var pedido = await _pedidoService.GetByIdAsync(id);
            if (pedido == null) return NotFound("Pedido no encontrado.");
            return Ok(pedido);
        }

        private int GetUserId()
        {
            return int.Parse(User.FindFirst("sub")?.Value ?? "0");
        }

        private bool IsAdmin()
        {
            return User.HasClaim(c => c.Type == "esAdmin" && c.Value == "");
        }
    }

}