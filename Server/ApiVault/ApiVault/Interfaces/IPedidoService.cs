using ApiVault.DTOs;

namespace ApiVault.Interfaces
{
    public interface IPedidoService
    {
        Task<IEnumerable<PedidoDto>> GetPedidosByUsuarioAsync(int idUsuario);
        Task<IEnumerable<PedidoDto>> GetTodosAsync(); //solo admins
        Task<PedidoDto> CrearPedidoDesdeCarritoAsync(int idUsuario, string metodoPago);
        Task<PedidoDto> GetByIdAsync(int id);
    }
}
