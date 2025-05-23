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
            Console.WriteLine($"🔵 Iniciando verificación de orden PayPal con ID: {dto.OrderId}");

            try
            {
                var esValida = await _paypal.VerificarOrdenAsync(dto.OrderId);
                Console.WriteLine($"🟢 Resultado de verificación de PayPal: {esValida}");

                if (!esValida)
                {
                    Console.WriteLine("❌ La orden no es válida según PayPal.");
                    return BadRequest("La orden no es válida");
                }

                // var userIdClaim = User.FindFirst("sub")?.Value;
                // Console.WriteLine($"🔐 Usuario autenticado con Claim 'sub': {userIdClaim}");

                var userIdClaim = User.FindFirst("sub")?.Value ?? User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                Console.WriteLine($"🔐 Usuario autenticado con Claim: {userIdClaim}");

                var usuarioId = int.TryParse(userIdClaim, out var parsedId) ? parsedId : 0;


                if (usuarioId == 0)
                {
                    Console.WriteLine("❌ UsuarioId inválido al verificar orden.");
                    return Unauthorized("Usuario no identificado");
                }

                var carrito = await _carritoService.GetCarritoEntityAsync(usuarioId);

                if (carrito == null)
                {
                    Console.WriteLine("❌ Carrito no encontrado para el usuario.");
                    return BadRequest("El carrito no existe.");
                }

                if (carrito.CarritoProductos == null || !carrito.CarritoProductos.Any())
                {
                    Console.WriteLine("❌ Carrito vacío.");
                    return BadRequest("El carrito está vacío.");
                }

                Console.WriteLine($"🛒 Carrito con {carrito.CarritoProductos.Count} productos. Procesando pedido...");

                var creado = await _pedidoService.CrearPedidoDesdeCarritoAsync(usuarioId, carrito);
                if (!creado)
                {
                    Console.WriteLine("❌ Error al crear pedido desde carrito.");
                    return StatusCode(500, "Error al crear el pedido.");
                }

                await _carritoService.ClearCarritoAsync(usuarioId);
                Console.WriteLine("✅ Pedido creado y carrito vaciado exitosamente.");

                return Ok(new { estado = "confirmado", mensaje = "Pago procesado y pedido creado con éxito" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Excepción en verificación de orden: {ex.Message}");
                return StatusCode(500, "Error interno al verificar la orden.");
            }
        }

    }
}