using ApiVault.DTOs;

namespace ApiVault.Interfaces
{
    public interface ITipoProductoService
    {
        Task<IEnumerable<TipoProductoDto>> GetAllAsync();
        Task<TipoProductoDto> GetByIdAsync(int id);
        Task<TipoProductoDto> CreateAsync(TipoProductoDto dto);
        Task<bool> UpdateAsync(int id, TipoProductoDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
