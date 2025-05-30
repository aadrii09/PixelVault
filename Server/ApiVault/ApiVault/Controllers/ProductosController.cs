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
        public async Task<ActionResult<ProductoDto>> Create([FromBody] ProductoDto dto)

        {
            try
            {
                Console.WriteLine("DTO recibido:");
                Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(dto));

                //if (imagen != null && imagen.length > 0)
                //{
                //    string[] allowedextensions = { ".jpg", ".png", ".svg" };
                //    string fileextension = path.getextension(imagen.filename).tolowerinvariant();

                //    if (!allowedextensions.contains(fileextension))
                //    {
                //        return badrequest("tipo de archivo no permitido. use imágenes jpg, png o svg.");
                //    }

                //    string imageurl = await _cloudinaryservice.uploadimageasync(imagen);
                //    dto.imagenurl = imageurl;
                //}

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
