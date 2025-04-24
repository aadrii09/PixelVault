using ApiVault.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiVault.Data 
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Historial> Historiales { get; set; }
        public DbSet<TipoProducto> TiposProductos { get; set; }
        public DbSet<Marca> Marcas { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Plataforma> Plataformas { get; set; }
        public DbSet<ProductoPlataforma> ProductoPlataformas { get; set; }
        public DbSet<Precio> Precios { get; set; }
        public DbSet<Comentario> Comentarios { get; set; }
        public DbSet<Carrito> Carritos { get; set; }
        public DbSet<CarritoProducto> CarritoProductos { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<PedidoDetalle> PedidoDetalles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuraciones adicionales si son necesarias

            // Ejemplo: configuración de claves compuestas
            // modelBuilder.Entity<ProductoPlataforma>()
            //     .HasKey(pp => new { pp.IdProducto, pp.IdPlataforma });

            // Ejemplo: configuración de relaciones
            // modelBuilder.Entity<Producto>()
            //     .HasOne(p => p.Marca)
            //     .WithMany(m => m.Productos)
            //     .HasForeignKey(p => p.IdMarca)
            //     .OnDelete(DeleteBehavior.Restrict);
        }
    }
}