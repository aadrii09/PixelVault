using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ApiVault.Models;

namespace ApiVault.Models
{
    public class Usuario
    {
        [Key]
        public int IdUsuario { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [Required]
        [StringLength(100)]
        public string Apellidos { get; set; }

        [Required]
        [StringLength(150)]
        [EmailAddress]
        public string Email { get; set; }

        [StringLength(255)]
        public string Direccion { get; set; }

        [StringLength(20)]
        public string Telefono { get; set; }

        [Required]
        public DateTime FechaRegistro { get; set; }

        [Required]
        public bool EsAdmin { get; set; }

        // Propiedades de navegación
        public virtual ICollection<Historial> Historiales { get; set; }
        public virtual ICollection<Comentario> Comentarios { get; set; }
        public virtual ICollection<Carrito> Carritos { get; set; }
        public virtual ICollection<Pedido> Pedidos { get; set; }
    }
}