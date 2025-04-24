using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ApiVault.Models;

namespace ApiVault.Models
{
    public class Historial
    {
        [Key]
        public int IdHistorial { get; set; }

        [Required]
        public int IdUsuario { get; set; }

        [Required]
        [StringLength(100)]
        public string Accion { get; set; }

        [Required]
        public DateTime FechaAccion { get; set; }

        [StringLength(50)]
        public string IpDireccion { get; set; }

        // Propiedad de navegación
        [ForeignKey("IdUsuario")]
        public virtual Usuario Usuario { get; set; }
    }
}