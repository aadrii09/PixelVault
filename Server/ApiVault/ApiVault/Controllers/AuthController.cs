using ApiVault.DTOs;
using ApiVault.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ApiVault.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register(RegistroDto registroDto)
        {
            var result = await _authService.RegisterAsync(registroDto);
            return Ok(new {message = result});    
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var result = await _authService.LoginAsync(loginDto);
            return Ok(new {token = result});
        }
    }
}
