namespace ApiVault.DTOs
{
    public class ProductoDto
    {
        public int IdProducto { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int Stock { get; set; }
        public DateTime FechaLanzamiento { get; set; }
        public string ImagenUrl { get; set; }
        public bool Activo { get; set; }    
        public bool Destacado { get; set; } 
        public int IdMarca { get; set; }
        public int IdTipo { get; set; }
    }
}
