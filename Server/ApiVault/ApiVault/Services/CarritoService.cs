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

        public async Task<CarritoDto> GetCarritoByUsurioAsync(int usuarioId)
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
                _context.CarritoProductos.Update(existente);
            }
            else
            {
                var nuevoProducto = new CarritoProducto
                {
                    IdCarrito = carrito.IdCarrito,
                    IdProducto = productoDto.IdProducto,
                    Cantidad = productoDto.Cantidad,
                    PrecioUnitario = productoDto.PrecioUnitario
                };
                carrito.CarritoProductos.Add(nuevoProducto);
            }
        }
    }
}
