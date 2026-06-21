using Microsoft.EntityFrameworkCore;
using WebMvcApp.Models;

namespace WebMvcApp.Data;

// Contexto para el contenedor MariaDB / MySQL.
public class MariaDbContext : DbContext
{
    public MariaDbContext(DbContextOptions<MariaDbContext> options) : base(options) { }

    public DbSet<Cliente> Clientes => Set<Cliente>();
    public DbSet<Producto> Productos => Set<Producto>();
    public DbSet<Pedido> Pedidos => Set<Pedido>();
    public DbSet<DetallePedido> DetallePedidos => Set<DetallePedido>();
}
