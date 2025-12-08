using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LaReservaBackend.Domain.Entities;
public class Complejo
{
    public long Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Nombre { get; set; } = null!;

    [MaxLength(200)]
    public string? Direccion { get; set; }

    public long CiudadId { get; set; }
    public Ciudad? Ciudad { get; set; }

    public long DuenoId { get; set; }
    public Usuario? Dueno { get; set; }

    public long? DeporteId { get; set; }
    public Deporte? Deporte { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal? Precio { get; set; }

    public string? Imagen { get; set; }

    [MaxLength(100)]
    public string? Categoria { get; set; }
    [MaxLength(100)]
    public string? DeportePillBg { get; set; }
    [MaxLength(100)]
    public string? DeportePillText { get; set; }

    public bool Estado { get; set; } = true;

    public DateTime FechaCreacion { get; set; }
    public int? MaxDiasDisponiblesReserva { get; set; }

    // Navegaciones
    public ICollection<Cancha> Canchas { get; set; } = new List<Cancha>();
    public ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
}
