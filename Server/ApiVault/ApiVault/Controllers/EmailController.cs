using ApiVault.Interfaces;
using ApiVault.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace ApiVault.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("AllowFrontend")]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _emailService;

        public EmailController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost("subscribe")]
        public async Task<ActionResult<EmailResponse>> Subscribe([FromBody] EmailRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new EmailResponse
                {
                    Success = false,
                    Message = "Email inválido"
                });
            }

            var success = await _emailService.SendWelcomeEmailAsync(request.Email);

            if (success)
            {
                return Ok(new EmailResponse
                {
                    Success = true,
                    Message = "¡Suscripción exitosa! Revisa tu correo."
                });
            }
            else
            {
                return StatusCode(500, new EmailResponse
                {
                    Success = false,
                    Message = "Error al enviar el correo. Inténtalo más tarde."
                });
            }
        }
    }

}
