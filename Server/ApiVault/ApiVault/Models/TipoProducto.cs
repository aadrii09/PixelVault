using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ApiVault.Models;

namespace ApiVault.Models
{
    public class TipoProducto
    {
        [Key]
        public int IdTipo { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [StringLength(255)]
        public string Descripcion { get; set; }

        // Propiedad de navegación
        public virtual ICollection<Producto> Productos { get; set; }
    }
}