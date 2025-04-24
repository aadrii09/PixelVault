using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ApiVault.Models;

namespace ApiVault.Models
{
    public class Carrito
    {
        [Key]
        public int IdCarrito { get; set; }

        [Required]
        public int IdUsuario { get; set; }

        [Required]
        public DateTime FechaCreacion { get; set; }

        [Required]
        [StringLength(50)]
        public string Estado { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Total { get; set; }

        // Propiedades de navegación
        [ForeignKey("IdUsuario")]
        public virtual Usuario Usuario { get; set; }

        public virtual ICollection<CarritoProducto> CarritoProductos { get; set; }

        // Constructor para inicializar la colección
        public Carrito()
        {
            CarritoProductos = new HashSet<CarritoProducto>();
        }
    }
}