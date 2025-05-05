using ApiVault.Data;
using ApiVault.DTOs;
using ApiVault.Interfaces;
using ApiVault.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiVault.Services
{
    public class MarcaService : IMarcaService
    {
        private readonly ApplicationDbContext _context;
        public MarcaService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MarcaDto>> GetAllAsync()
        {
            return await _context.Marcas.Select(m => new MarcaDto
            {
                IdMarca = m.IdMarca,
                Nombre = m.Nombre,
                LogoUrl = m.LogoUrl,
                Website = m.Website
            }).ToListAsync();
        }

        public async Task<MarcaDto?> GetByIdAsync(int id)
        {
            var marca = await _context.Marcas.FindAsync(id);    
            return marca == null ? null : new MarcaDto
            {
                IdMarca = marca.IdMarca,
                Nombre = marca.Nombre,
                LogoUrl = marca.LogoUrl,
                Website = marca.Website
            };
        }

        public async Task<MarcaDto> CreateAsync(MarcaDto dto)
        {
            var marca = new Marca
            {
                Nombre = dto.Nombre,
                LogoUrl = dto.LogoUrl,
                Website = dto.Website
            };
            _context.Marcas.Add(marca);
            await _context.SaveChangesAsync();
            dto.IdMarca = marca.IdMarca;
            return dto;
        }

        public async Task<bool> UpdateAsync(int id, MarcaDto dto)
        {
            var marca = await _context.Marcas.FindAsync(id);
            if (marca == null) return false;

            marca.Nombre = dto.Nombre;
            marca.LogoUrl = dto.LogoUrl;
            marca.Website = dto.Website;
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var marca = await _context.Marcas.FindAsync(id);
            if (marca == null) return false;
            _context.Marcas.Remove(marca);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
