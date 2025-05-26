using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ApiVault.Services;
using ApiVault.DTOs;

namespace ApiVault.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class StripeController : ControllerBase
    {
        private readonly StripeService _stripeService;

        public StripeController(StripeService stripeService)
        {
            _stripeService = stripeService;
        }

        [HttpPost("crear-intent")]
        public async Task<IActionResult> CreatePaymentIntent([FromBody] PaymentIntentDto dto)
        {
            try
            {
                var clientSecret = await _stripeService.CreatePaymentIntentAsync(dto.Amount);
                return Ok(new { clientSecret });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al crear el intent de pago: {ex.Message}");
            }
        }

        [HttpPost("verificar-pago")]
        public async Task<IActionResult> VerifyPayment([FromBody] VerifyPaymentDto dto)
        {
            try
            {
                var isSuccess = await _stripeService.VerifyPaymentAsync(dto.PaymentIntentId);
                return Ok(new { success = isSuccess });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al verificar el pago: {ex.Message}");
            }
        }
    }
}