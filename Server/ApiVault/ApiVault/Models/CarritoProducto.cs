using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ApiVault.Models;

namespace ApiVault.Models
{
    public class CarritoProducto
    {
        [Key]
        public int IdCarritoProducto { get; set; }

        [Required]
        public int IdCarrito { get; set; }

        [Required]
        public int IdProducto { get; set; }

        [Required]
        public int Cantidad { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal PrecioUnitario { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Subtotal { get; set; }

        // Propiedades de navegación
        [ForeignKey("IdCarrito")]
        public virtual Carrito Carrito { get; set; }

        [ForeignKey("IdProducto")]
        public virtual Producto Producto { get; set; }
    }
}