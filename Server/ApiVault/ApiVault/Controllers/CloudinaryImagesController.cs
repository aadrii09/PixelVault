using Microsoft.AspNetCore.Mvc;
using ApiVault.Services;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ApiVault.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CloudinaryImagesController : ControllerBase
    {
        private readonly CloudinaryService _cloudinaryService;

        public CloudinaryImagesController(CloudinaryService cloudinaryService)
        {
            _cloudinaryService = cloudinaryService;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadImage(IFormFile image)
        {
            if (image == null || image.Length == 0)
            {
                return BadRequest("No se ha enviado ninguna imagen o la imagen está vacía.");
            }

            try
            {
                // Validamos el tipo de archivo (opcional)
                string[] allowedExtensions = { ".jpg", ".png", ".svg" };
                string fileExtension = Path.GetExtension(image.FileName).ToLowerInvariant();

                if (!allowedExtensions.Contains(fileExtension))
                {
                    return BadRequest("Tipo de archivo no permitido. Use imágenes JPG, PNG o GIF.");
                }

                // Subimos la imagen a Cloudinary
                string imageUrl = await _cloudinaryService.UploadImageAsync(image);

                return Ok(new { url = imageUrl });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al subir la imagen: {ex.Message}");
            }
        }
    }
}
