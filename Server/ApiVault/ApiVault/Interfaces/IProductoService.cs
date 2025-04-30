using ApiVault.DTOs;

namespace ApiVault.Interfaces
{
    public interface IProductoService
    {
        Task<IEnumerable<ProductoDto>> GetAllAsync();
        Task<ProductoDto> GetByIdAsync(int id);
        Task<ProductoDto> CreateAsync(ProductoDto dto);
        Task<bool> UpdateAsync(int id, ProductoDto dto);
        Task<bool> DeleteAsync(int id);


        Task<IEnumerable<ProductoDto>> GetAllProductoAsync();
        Task<ProductoDto> GetProductoByIdAsync(int id);
    }
}
