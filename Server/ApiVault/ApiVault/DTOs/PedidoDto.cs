namespace ApiVault.DTOs
{
    public class PedidoDto
    {
        public int IdPedido { get; set; }
        public int IdUsuario { get; set; }
        public DateTime FechaPedido { get; set; }
        public string EstadoPedido { get; set; }
        public decimal Total { get; set; }
        public string MetodoPago { get; set; }
        public List<PedidoDetalleDto> Detalles { get; set; }
    }
}
