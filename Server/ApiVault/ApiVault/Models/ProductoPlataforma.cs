using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiVault.Models
{
    public class ProductoPlataforma
    {
        [Key]
        public int IdProductPlataforma { get; set; }

        [Required]
        public int IdProducto { get; set; }

        [Required]
        public int IdPlataforma { get; set; }

        // Propiedades de navegación
        [ForeignKey("IdProducto")]
        public virtual Producto Producto { get; set; }

        [ForeignKey("IdPlataforma")]
        public virtual Plataforma Plataforma { get; set; }
    }
}