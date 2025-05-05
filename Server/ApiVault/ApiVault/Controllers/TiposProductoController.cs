using Microsoft.AspNetCore.Mvc;
using ApiVault.Interfaces;
using ApiVault.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace ApiVault.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TiposProductosController : ControllerBase
    {
        private readonly ITipoProductoService _tipoService;

        public TiposProductosController(ITipoProductoService tipoService)
        {
            _tipoService = tipoService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var tipos = await _tipoService.GetAllAsync();
            return Ok(tipos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var tipo = await _tipoService.GetByIdAsync(id);
            if (tipo == null) return NotFound();
            return Ok(tipo);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> Create(TipoProductoDto dto)
        {
            var t = await _tipoService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = t.IdTipo }, t);
        }

        [Authorize(Roles = "admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, TipoProductoDto dto)
        {
            var ok = await _tipoService.UpdateAsync(id, dto);
            return ok ? NoContent() : NotFound();
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _tipoService.DeleteAsync(id);
            return ok ? NoContent() : NotFound();
        }
    }
}