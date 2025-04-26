using ApiVault.DTOs;

namespace ApiVault.Interfaces
{
    public interface IProductoService
    {
        Task<IEnumerable<ProductoDto>> GetAllProductoAsync();
        Task<ProductoDto> GetProductoByIdAsync(int id);
    }
}
