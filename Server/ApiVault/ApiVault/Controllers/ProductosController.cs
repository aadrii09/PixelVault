using ApiVault.DTOs;
using ApiVault.Interfaces;
using ApiVault.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace ApiVault.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosController : ControllerBase
    {
        private readonly IProductoService _productoService;
        private readonly CloudinaryService _cloudinaryService;

        public ProductosController(IProductoService productoService, CloudinaryService cloudinaryService)
        {
            _productoService = productoService;
            _cloudinaryService = cloudinaryService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductoDto>>> GetAll()
        {
            var productos = await _productoService.GetAllAsync();
            return Ok(productos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductoDto>> GetById(int id)
        {
            var producto = await _productoService.GetByIdAsync(id);
            if (producto == null)
            {
                return NotFound();
            }
            return Ok(producto);
        }

        [Authorize(Roles = "Admin")] //jwt
        [HttpPost]
        public async Task<ActionResult<ProductoDto>> Create([FromForm] ProductoDto dto, IFormFile imagen)
        {
            try
            {
                // Si hay una imagen, procesarla
                if (imagen != null && imagen.Length > 0)
                {
                    // Validamos el tipo de archivo
                    string[] allowedExtensions = { ".jpg", ".png", ".svg" };
                    string fileExtension = Path.GetExtension(imagen.FileName).ToLowerInvariant();

                    if (!allowedExtensions.Contains(fileExtension))
                    {
                        return BadRequest("Tipo de archivo no permitido. Use imágenes JPG, PNG o SVG.");
                    }

                    // Subimos la imagen a Cloudinary
                    string imageUrl = await _cloudinaryService.UploadImageAsync(imagen);

                    // Asignar la URL de la imagen al DTO
                    dto.ImagenUrl = imageUrl;
                }

                // Crear el producto
                var creado = await _productoService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = creado.IdProducto }, creado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al crear el producto: {ex.Message}");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ProductoDto dto)
        {
            var actualizado = await _productoService.UpdateAsync(id, dto);
            if (!actualizado)
            {
                return NotFound();
            }
            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var eliminado = await _productoService.DeleteAsync(id);
            if (!eliminado)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
