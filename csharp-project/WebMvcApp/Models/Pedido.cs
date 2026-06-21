using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebMvcApp.Models;

public class Pedido
{
    [Key]
    public int PedidoId { get; set; }

    [Required]
    public int ClienteId { get; set; }

    public DateTime FechaPedido { get; set; } = DateTime.Now;

    [Column(TypeName = "decimal(10,2)")]
    public decimal Total { get; set; }

    [Required, StringLength(30)]
    public string Estado { get; set; } = "Pendiente";

    [ForeignKey(nameof(ClienteId))]
    public Cliente? Cliente { get; set; }

    public ICollection<DetallePedido>? Detalles { get; set; }
}
