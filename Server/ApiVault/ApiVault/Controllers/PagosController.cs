using ApiVault.DTOs;
using ApiVault.Interfaces;
using ApiVault.Models;
using ApiVault.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiVault.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PagosController : ControllerBase
    {
        private readonly PaypalService _paypal;
        private readonly ICarritoService _carritoService;
        private readonly IPedidoService _pedidoService;

        public PagosController(PaypalService paypal, ICarritoService carritoService, IPedidoService pedidoService)
        {
            _paypal = paypal;
            _carritoService = carritoService;
            _pedidoService = pedidoService;
        }

        [HttpPost("crear-orden")]
        public async Task<IActionResult> CrearOrden([FromBody] MontoDto dto)
        {
            try
            {
                Console.WriteLine($"Recibida solicitud para crear orden PayPal con total: {dto.Total} {dto.Currency}");
                var orderId = await _paypal.CrearOrderAsync(dto);
                Console.WriteLine($"Orden creada con ID: {orderId}");
                return Ok(new { orderId });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al crear orden PayPal: {ex.Message}");
                return StatusCode(500, new { mensaje = "Error interno al crear la orden", error = ex.Message });
            }
        }


        [HttpPost("verificar-orden")]
        public async Task<IActionResult> VerificarOrden([FromBody] VerificarDto dto)
        {
            var esValida = await _paypal.VerificarOrdenAsync(dto.OrderId);
            if (!esValida)
                return BadRequest("La orden no es válida");

            var usuarioId = int.Parse(User.FindFirst("sub")?.Value ?? "0");

            // Obtener el carrito original
            var carrito = await _carritoService.GetCarritoEntityAsync(usuarioId);
            if (carrito == null || carrito.CarritoProductos == null || !carrito.CarritoProductos.Any())
                return BadRequest("El carrito está vacío o no existe.");

            // Crear el pedido
            var creado = await _pedidoService.CrearPedidoDesdeCarritoAsync(usuarioId, carrito);
            if (!creado)
                return StatusCode(500, "Error al crear el pedido.");

            // Vaciar carrito
            await _carritoService.ClearCarritoAsync(usuarioId);

            return Ok(new { estado = "confirmado", mensaje = "Pago procesado y pedido creado con éxito" });
        }
    }
}