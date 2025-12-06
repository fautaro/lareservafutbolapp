using System.ComponentModel.DataAnnotations;

namespace LaReservaBackend.Domain.Entities;
public class TipoCancha
{
    public long Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string Nombre { get; set; } = null!;

    // Navegación
    public ICollection<Cancha> Canchas { get; set; } = new List<Cancha>();
}
