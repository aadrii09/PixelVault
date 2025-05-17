using ApiVault.Data;
using ApiVault.DTOs;
using ApiVault.Interfaces;
using ApiVault.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiVault.Services
{
    public class PedidoService : IPedidoService
    {
        private readonly ApplicationDbContext _context;

        public PedidoService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PedidoDto> CrearPedidoDesdeCarritoAsync(int idUsuario, string metodoPago)
        {
            var carrito = await _context.Carritos
                .Include(c => c.CarritoProductos)
                .FirstOrDefaultAsync(c => c.IdUsuario == idUsuario && c.Estado == "Abierto");

            if (carrito == null || !carrito.CarritoProductos.Any())
                return null;

            var pedido = new Pedido
            {
                IdUsuario = idUsuario,
                FechaPedido = DateTime.UtcNow,
                EstadoPedido = "Procesando",
                MetodoPago = metodoPago,
                Total = carrito.Total,
                PedidoDetalles = carrito.CarritoProductos.Select(cp => new PedidoDetalle
                {
                    IdProducto = cp.IdProducto,
                    Cantidad = cp.Cantidad,
                    PrecioUnitario = cp.PrecioUnitario,
                    Subtotal = cp.Subtotal,
                }).ToList()
            };

            _context.Pedidos.Add(pedido);

            carrito.Estado = "Finalizado";
            _context.CarritoProductos.RemoveRange(carrito.CarritoProductos);
            await _context.SaveChangesAsync();

            return await GetByIdAsync(pedido.IdPedido);
        }

        public async Task<bool> CrearPedidoDesdeCarritoAsync(int idUsuario, Carrito carrito)
        {
            if (carrito == null || carrito.CarritoProductos == null || !carrito.CarritoProductos.Any())
                return false;

            var pedido = new Pedido
            {
                IdUsuario = idUsuario,
                FechaPedido = DateTime.UtcNow,
                EstadoPedido = "Pagado",
                MetodoPago = "PayPal",
                Total = carrito.Total,
                PedidoDetalles = carrito.CarritoProductos.Select(cp => new PedidoDetalle
                {
                    IdProducto = cp.IdProducto,
                    Cantidad = cp.Cantidad,
                    PrecioUnitario = cp.PrecioUnitario,
                    Subtotal = cp.Subtotal,
                }).ToList()
            };

            _context.Pedidos.Add(pedido);

            carrito.Estado = "Finalizado";
            _context.CarritoProductos.RemoveRange(carrito.CarritoProductos);

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<PedidoDto> GetByIdAsync(int id)
        {
            var pedido = await _context.Pedidos
                .Include(p => p.PedidoDetalles)
                .ThenInclude(d => d.Producto)
                .FirstOrDefaultAsync(p => p.IdPedido == id);

            if (pedido == null) return null;

            return new PedidoDto
            {
                IdPedido = pedido.IdPedido,
                IdUsuario = pedido.IdUsuario,
                FechaPedido = pedido.FechaPedido,
                EstadoPedido = pedido.EstadoPedido,
                Total = pedido.Total,
                MetodoPago = pedido.MetodoPago,
                Detalles = pedido.PedidoDetalles.Select(d => new PedidoDetalleDto
                {
                    IdProducto = d.IdProducto,
                    NombreProducto = d.Producto.Nombre,
                    Cantidad = d.Cantidad,
                    PrecioUnitario = d.PrecioUnitario,
                }).ToList()
            };
        }

        public async Task<IEnumerable<PedidoDto>> GetPedidosByUsuarioAsync(int idUsuario)
        {
            return await _context.Pedidos
                .Where(p => p.IdUsuario == idUsuario)
                .OrderByDescending(p => p.FechaPedido)
                .Select(p => new PedidoDto
                {
                    IdPedido = p.IdPedido,
                    FechaPedido = p.FechaPedido,
                    EstadoPedido = p.EstadoPedido,
                    Total = p.Total,
                    MetodoPago = p.MetodoPago,
                }).ToListAsync();
        }

        public async Task<IEnumerable<PedidoDto>> GetTodosAsync()
        {
            return await _context.Pedidos
                .Include(p => p.Usuario)
                .OrderByDescending(p => p.FechaPedido)
                .Select(p => new PedidoDto
                {
                    IdPedido = p.IdPedido,
                    IdUsuario = p.IdUsuario,
                    FechaPedido = p.FechaPedido,
                    EstadoPedido = p.EstadoPedido,
                    Total = p.Total,
                    MetodoPago = p.MetodoPago,
                }).ToListAsync();
        }
    }
}