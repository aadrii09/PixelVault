namespace ApiVault.DTOs
{
    public class CarritoProductoDto
    {
        public int IdProducto { get; set; }
        public string Nombre { get; set; }            
        public string ImagenUrl { get; set; }         
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal => Cantidad * PrecioUnitario;
    }

}
