using Microsoft.EntityFrameworkCore;
using WebMvcApp.Models;

namespace WebMvcApp.Data;

// Contexto para el contenedor SQL Server.
public class SqlServerContext : DbContext
{
    public SqlServerContext(DbContextOptions<SqlServerContext> options) : base(options) { }

    public DbSet<Cliente> Clientes => Set<Cliente>();
    public DbSet<Producto> Productos => Set<Producto>();
    public DbSet<Pedido> Pedidos => Set<Pedido>();
    public DbSet<DetallePedido> DetallePedidos => Set<DetallePedido>();
}
