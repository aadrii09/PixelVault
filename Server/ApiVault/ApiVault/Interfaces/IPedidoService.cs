using ApiVault.DTOs;
using ApiVault.Models;

namespace ApiVault.Interfaces
{
    public interface IPedidoService
    {
        Task<PedidoDto> CrearPedidoDesdeCarritoAsync(int idUsuario, string metodoPago);
        Task<bool> CrearPedidoDesdeCarritoAsync(int idUsuario, Carrito carrito);
        Task<PedidoDto> GetByIdAsync(int id);
        Task<IEnumerable<PedidoDto>> GetPedidosByUsuarioAsync(int idUsuario);
        Task<IEnumerable<PedidoDto>> GetTodosAsync();
    }
}