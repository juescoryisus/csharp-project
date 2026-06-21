using System.ComponentModel.DataAnnotations;

namespace WebMvcApp.Models;

public class Cliente
{
    [Key]
    public int ClienteId { get; set; }

    [Required, StringLength(100)]
    public string Nombre { get; set; } = string.Empty;

    [Required, StringLength(150), EmailAddress]
    public string Email { get; set; } = string.Empty;

    [StringLength(20)]
    public string? Telefono { get; set; }

    [StringLength(80)]
    public string? Ciudad { get; set; }

    [DataType(DataType.Date)]
    public DateTime FechaRegistro { get; set; } = DateTime.Today;

    public ICollection<Pedido>? Pedidos { get; set; }
}
