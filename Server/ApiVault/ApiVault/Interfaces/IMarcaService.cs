using ApiVault.DTOs;

namespace ApiVault.Interfaces
{
    public interface IMarcaService
    {
        Task<IEnumerable<MarcaDto>> GetAllAsync();
        Task<MarcaDto> GetByIdAsync(int id);
        Task<MarcaDto> CreateAsync(MarcaDto dto);
        Task<bool> UpdateAsync(int id, MarcaDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
