using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebMvcApp.Models;

public class Producto
{
    [Key]
    public int ProductoId { get; set; }

    [Required, StringLength(120)]
    public string Nombre { get; set; } = string.Empty;

    [StringLength(255)]
    public string? Descripcion { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal Precio { get; set; }

    public int Stock { get; set; }

    [StringLength(60)]
    public string? Categoria { get; set; }

    public ICollection<DetallePedido>? Detalles { get; set; }
}
