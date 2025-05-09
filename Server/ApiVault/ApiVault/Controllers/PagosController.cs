using ApiVault.DTOs;
using ApiVault.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiVault.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PagosController : ControllerBase
    {
        private readonly PaypalService _paypal;
        public PagosController(PaypalService paypal)
        {
            _paypal = paypal;
        }

        [HttpPost("crear-orden")]
        public async Task<IActionResult> CrearOrden([FromBody] MontoDto dto)
        {
            var orderId = await _paypal.CrearOrderAsync(dto);
            return Ok(new { orderId });
        }
        [HttpPost("verificar-orden")]
        public async Task<IActionResult> VerificarOrden([FromBody] VerificarDto dto)
        {
            var esValida= await _paypal.VerificarOrdenAsync(dto.OrderId);
            if(!esValida)
            {
                return BadRequest("La orden no es válida");
            }
            return Ok(new {estado = "confirmado"});
        }
    }
}
