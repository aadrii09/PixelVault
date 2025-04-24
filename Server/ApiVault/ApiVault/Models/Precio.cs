using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiVault.Models
{
    public class Precio
    {
        [Key]
        public int IdPrecio { get; set; }

        [Required]
        public int IdProducto { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal PrecioRegular { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal PrecioOferta { get; set; }

        public DateTime? FechaInicioOferta { get; set; }

        public DateTime? FechaFinOferta { get; set; }

        // Propiedad de navegación
        [ForeignKey("IdProducto")]
        public virtual Producto Producto { get; set; }
    }
}