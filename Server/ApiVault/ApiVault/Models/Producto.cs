using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ApiVault.Models;

namespace ApiVault.Models
{
    public class Producto
    {
        [Key]
        public int IdProducto { get; set; }

        [Required]
        [StringLength(150)]
        public string Nombre { get; set; }

        [Column(TypeName = "text")]
        public string Descripcion { get; set; }

        [Required]
        public int Stock { get; set; }

        [Column(TypeName = "date")]
        public DateTime FechaLanzamiento { get; set; }

        [StringLength(255)]
        public string ImagenUrl { get; set; }

        [Required]
        public bool Activo { get; set; }

        [Required]
        public bool Destacado { get; set; }

        [Required]
        public int IdMarca { get; set; }

        [Required]
        public int IdTipo { get; set; }

        // Propiedades de navegación
        [ForeignKey("IdMarca")]
        public virtual Marca Marca { get; set; }

        [ForeignKey("IdTipo")]
        public virtual TipoProducto TipoProducto { get; set; }

        public virtual ICollection<Precio> Precios { get; set; }
        public virtual ICollection<ProductoPlataforma> ProductoPlataformas { get; set; }
        public virtual ICollection<Comentario> Comentarios { get; set; }
        public virtual ICollection<CarritoProducto> CarritoProductos { get; set; }
        public virtual ICollection<PedidoDetalle> PedidoDetalles { get; set; }
    }
}