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

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<ProductoDto>> Create([FromForm] ProductoDto dto, IFormFile imagen)
        {
            try
            {
                Console.WriteLine("DTO recibido:");
                Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(dto));

                if (imagen != null && imagen.Length > 0)
                {
                    string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".svg" };
                    string fileExtension = Path.GetExtension(imagen.FileName).ToLowerInvariant();

                    if (!allowedExtensions.Contains(fileExtension))
                    {
                        return BadRequest("Tipo de archivo no permitido. Use imágenes JPG, PNG o SVG.");
                    }

                    string imageUrl = await _cloudinaryService.UploadImageAsync(imagen);
                    dto.ImagenUrl = imageUrl;
                    Console.WriteLine($"Imagen subida a Cloudinary: {imageUrl}");
                }

                var creado = await _productoService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = creado.IdProducto }, creado);
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Error: " + ex.Message);
                return StatusCode(500, $"Error al crear el producto: {ex.Message}");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] ProductoDto dto, IFormFile imagen)
        {
            try
            {
                if (imagen != null && imagen.Length > 0)
                {
                    string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".svg" };
                    string fileExtension = Path.GetExtension(imagen.FileName).ToLowerInvariant();

                    if (!allowedExtensions.Contains(fileExtension))
                    {
                        return BadRequest("Tipo de archivo no permitido. Use imágenes JPG, PNG o SVG.");
                    }

                    string imageUrl = await _cloudinaryService.UploadImageAsync(imagen);
                    dto.ImagenUrl = imageUrl;
                }

                var actualizado = await _productoService.UpdateAsync(id, dto);
                if (!actualizado)
                {
                    return NotFound();
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Error: " + ex.Message);
                return StatusCode(500, $"Error al actualizar el producto: {ex.Message}");
            }
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
