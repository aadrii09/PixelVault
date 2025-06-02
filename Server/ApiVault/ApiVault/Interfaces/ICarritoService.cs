using ApiVault.DTOs;
using ApiVault.Models;

namespace ApiVault.Interfaces
{
    public interface ICarritoService
    {
        Task<CarritoDto> GetCarritoByUsuarioAsync(int idUsuario);
        Task<Carrito> GetCarritoEntityAsync(int usuarioId);
        Task<CarritoDto> AddProductoAsync(int usuarioId, CarritoProductoDto productoDto);
        Task<bool> RemoveProductoAsync(int usuarioId, int productoId);
        Task<bool> ClearCarritoAsync(int usuarioId);
        Task<bool> ActualizarCantidadProductoAsync(int usuarioId, int productoId, int cantidad);
    }
}
