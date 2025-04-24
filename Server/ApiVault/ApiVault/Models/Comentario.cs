using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ApiVault.Models;

namespace ApiVault.Models
{
    public class Comentario
    {
        [Key]
        public int IdComentario { get; set; }

        [Required]
        public int IdProducto { get; set; }

        [Required]
        public int IdUsuario { get; set; }

        [Required]
        [Column(TypeName = "text")]
        public string TextoComentario { get; set; }

        [Required]
        [Range(1, 5)]
        public byte Valoracion { get; set; }

        [Required]
        public DateTime FechaComentario { get; set; }

        [Required]
        public bool Aprobado { get; set; }

        // Propiedades de navegación
        [ForeignKey("IdProducto")]
        public virtual Producto Producto { get; set; }

        [ForeignKey("IdUsuario")]
        public virtual Usuario Usuario { get; set; }
    }
}