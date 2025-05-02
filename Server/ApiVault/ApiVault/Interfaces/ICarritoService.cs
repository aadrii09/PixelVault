using ApiVault.DTOs;

namespace ApiVault.Interfaces
{
    public interface ICarritoService
    {
        Task<List<CarritoDto>> GetCarritoByUsuarioAsync(int idUsuario);
        Task<CarritoDto> AddProductoAsync(int usuarioId, CarritoProductoDto productoDto);
        Task<bool> RemoveProductoAsync(int usuarioId, int productoId);
        Task<bool> ClearCarritoAsync(int usuarioId);
    }
}
