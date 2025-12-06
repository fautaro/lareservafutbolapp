using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LaReservaBackend.Domain.Entities;
public class Cancha
{
    public long Id { get; set; }

    public long ComplejoId { get; set; }
    public Complejo? Complejo { get; set; }

    public long TipoCanchaId { get; set; }
    public TipoCancha? TipoCancha { get; set; }

    [Required]
    [MaxLength(100)]
    public string Nombre { get; set; } = null!;

    [Column(TypeName = "decimal(10,2)")]
    public decimal? PrecioHora { get; set; }

    public bool Estado { get; set; } = true;

    public string? Descripcion { get; set; }

    // Navegación
    public ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
}
