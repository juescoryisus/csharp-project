using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebMvcApp.Models;

public class DetallePedido
{
    [Key]
    public int DetalleId { get; set; }

    [Required]
    public int PedidoId { get; set; }

    [Required]
    public int ProductoId { get; set; }

    public int Cantidad { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal PrecioUnitario { get; set; }

    [ForeignKey(nameof(PedidoId))]
    public Pedido? Pedido { get; set; }

    [ForeignKey(nameof(ProductoId))]
    public Producto? Producto { get; set; }
}
