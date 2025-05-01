using ApiVault.Data;
using ApiVault.DTOs;
using ApiVault.Interfaces;
using ApiVault.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiVault.Services
{
    public class ProductoService : IProductoService
    {
        private readonly ApplicationDbContext _context;

        public ProductoService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProductoDto>> GetAllAsync()
        {
            return await _context.Productos
                .Select(producto => new ProductoDto
                {
                    IdProducto = producto.IdProducto,
                    Nombre = producto.Nombre,
                    Descripcion = producto.Descripcion,
                    Stock = producto.Stock,
                    FechaLanzamiento = producto.FechaLanzamiento,
                    ImagenUrl = producto.ImagenUrl,
                    Activo = producto.Activo,
                    Destacado = producto.Destacado,
                    IdMarca = producto.IdMarca,
                    IdTipo = producto.IdTipo,
                })
                .ToListAsync();
        }

        public async Task<ProductoDto> GetByIdAsync(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
            {
                return null;
            }
            return new ProductoDto
            {
                IdProducto = producto.IdProducto,
                Nombre = producto.Nombre,
                Descripcion = producto.Descripcion,
                Stock = producto.Stock,
                FechaLanzamiento = producto.FechaLanzamiento,
                ImagenUrl = producto.ImagenUrl,
                Activo = producto.Activo,
                Destacado = producto.Destacado,
                IdMarca = producto.IdMarca,
                IdTipo = producto.IdTipo,
            };
        }

        public async Task<ProductoDto> CreateAsync(ProductoDto dto)
        {
            var producto = new Producto
            {
                Nombre = dto.Nombre,
                Descripcion = dto.Descripcion,
                Stock = dto.Stock,
                FechaLanzamiento = dto.FechaLanzamiento,
                ImagenUrl = dto.ImagenUrl,
                Activo = dto.Activo,
                Destacado = dto.Destacado,
                IdMarca = dto.IdMarca,
                IdTipo = dto.IdTipo,
            };
            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();

            return dto;

        }

        public async Task<bool> UpdateAsync(int id, ProductoDto dto)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
            {
                return false;
            }
            producto.Nombre = dto.Nombre;
            producto.Descripcion = dto.Descripcion;
            producto.Stock = dto.Stock;
            producto.FechaLanzamiento = dto.FechaLanzamiento;
            producto.ImagenUrl = dto.ImagenUrl;
            producto.Activo = dto.Activo;
            producto.Destacado = dto.Destacado;
            producto.IdMarca = dto.IdMarca;
            producto.IdTipo = dto.IdTipo;


            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
            {
                return false;
            }

            _context.Productos.Remove(producto);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
