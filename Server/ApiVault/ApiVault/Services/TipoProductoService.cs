using ApiVault.Data;
using ApiVault.DTOs;
using ApiVault.Interfaces;
using ApiVault.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiVault.Services
{
    public class TipoProductoService : ITipoProductoService
    {
        private readonly ApplicationDbContext _context;
        public TipoProductoService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TipoProductoDto>> GetAllAsync()
        {
            return await _context.TiposProductos.Select(t => new TipoProductoDto
            {
                IdTipo = t.IdTipo,
                Nombre = t.Nombre,
                Descripcion = t.Descripcion
            }).ToListAsync();
        }

        public async Task<TipoProductoDto?> GetByIdAsync(int id)
        {
            var tipoProducto = await _context.TiposProductos.FindAsync(id);
            return tipoProducto == null ? null : new TipoProductoDto
            {
                IdTipo = tipoProducto.IdTipo,
                Nombre = tipoProducto.Nombre,
                Descripcion = tipoProducto.Descripcion
            };
        }

        public async Task<TipoProductoDto> CreateAsync(TipoProductoDto dto)
        {
            var tipoProducto = new TipoProducto
            {
                Nombre = dto.Nombre,
                Descripcion = dto.Descripcion
            };
            _context.TiposProductos.Add(tipoProducto);
            await _context.SaveChangesAsync();
            dto.IdTipo = tipoProducto.IdTipo;
            return dto;
        }

        public async Task<bool> UpdateAsync(int id, TipoProductoDto dto)
        {
            var tipoProducto = await _context.TiposProductos.FindAsync(id);
            if (tipoProducto == null) return false;

            tipoProducto.Nombre = dto.Nombre;
            tipoProducto.Descripcion = dto.Descripcion;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var tipoProducto = await _context.TiposProductos.FindAsync(id);
            if (tipoProducto == null) return false;

            _context.TiposProductos.Remove(tipoProducto);

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
