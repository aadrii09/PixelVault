using ApiVault.Data;
using ApiVault.DTOs;
using ApiVault.Interfaces;
using ApiVault.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiVault.Services
{
    public class CarritoService : ICarritoService
    {
        private readonly ApplicationDbContext _context;
        public CarritoService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<CarritoDto> GetCarritoByUsuarioAsync(int usuarioId)
        {
            var carrito = await _context.Carritos
                .Include(c => c.CarritoProductos)
                .ThenInclude(cp => cp.Producto)
                .FirstOrDefaultAsync(c => c.IdUsuario == usuarioId && c.Estado == "Abierto");

            if (carrito == null)
            {
                return null;
            }
            return new CarritoDto
            {
                IdCarrito = carrito.IdCarrito,
                IdUsuario = usuarioId,
                FechaCreacion = carrito.FechaCreacion,
                Estado = carrito.Estado,
                Total = carrito.Total,
                Productos = carrito.CarritoProductos.Select(cp => new CarritoProductoDto
                {
                    IdProducto = cp.IdProducto,
                    Cantidad = cp.Cantidad,
                    PrecioUnitario = cp.PrecioUnitario
                }).ToList()
            };
        }

        public async Task<CarritoDto> AddProductoAsync(int usuarioId, CarritoProductoDto productoDto)
        {
            var carrito = await _context.Carritos
                .Include(c => c.CarritoProductos)
                .FirstOrDefaultAsync(c => c.IdUsuario == usuarioId && c.Estado == "Abierto");
            if (carrito == null)
            {
                carrito = new Carrito
                {
                    IdUsuario = usuarioId,
                    FechaCreacion = DateTime.Now,
                    Estado = "Abierto",
                    Total = 0m
                };
                _context.Carritos.Add(carrito);
                await _context.SaveChangesAsync();
            }

            var existente = carrito.CarritoProductos.FirstOrDefault(cp => cp.IdProducto == productoDto.IdProducto);

            if (existente != null)
            {
                existente.Cantidad += productoDto.Cantidad;
                existente.Subtotal = existente.Cantidad * productoDto.PrecioUnitario;
            }
            else
            {
                carrito.CarritoProductos.Add(new CarritoProducto
                {
                    IdProducto = productoDto.IdProducto,
                    Cantidad = productoDto.Cantidad,
                    PrecioUnitario = productoDto.PrecioUnitario,
                    Subtotal = productoDto.Cantidad * productoDto.PrecioUnitario
                });
            }

            carrito.Total = carrito.CarritoProductos.Sum(cp => cp.Subtotal);
            await _context.SaveChangesAsync();

            return await GetCarritoByUsuarioAsync(usuarioId);
        }

        public async Task<bool> RemoveProductoAsync(int usuarioId, int productoId)
        {
            var carrito = await _context.Carritos
                .Include(c => c.CarritoProductos)
                .FirstOrDefaultAsync(c => c.IdUsuario == usuarioId && c.Estado == "Abierto");
            if (carrito == null)
            {
                return false;
            }
            var producto = carrito.CarritoProductos.FirstOrDefault(cp => cp.IdProducto == productoId);
            if (producto == null)
            {
                return false;
            }
            _context.CarritoProductos.Remove(producto);
            await _context.SaveChangesAsync();

            carrito.Total = carrito.CarritoProductos.Sum(cp => cp.Subtotal);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ClearCarritoAsync(int usuarioId)
        {
            var carrito = await _context.Carritos
                .Include(c => c.CarritoProductos)
                .FirstOrDefaultAsync(c => c.IdUsuario == usuarioId && c.Estado == "Abierto");
            if (carrito == null)
            {
                return false;
            }
            _context.CarritoProductos.RemoveRange(carrito.CarritoProductos);
            carrito.Total = 0;
            await _context.SaveChangesAsync();
            return true;
        }

        
    }
}