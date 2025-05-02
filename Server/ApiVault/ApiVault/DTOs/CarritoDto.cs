using ApiVault.Models;

namespace ApiVault.DTOs
{
    public class CarritoDto
    {
        public int IdCarrito { get; set; }
        public int IdUsuario { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string Estado { get; set; }
        public decimal Total { get; set; }

        public List<CarritoProductoDto> Productos { get; set; } = new();
    }
}
