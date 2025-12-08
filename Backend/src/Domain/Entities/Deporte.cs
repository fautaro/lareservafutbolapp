using System.ComponentModel.DataAnnotations;

namespace LaReservaBackend.Domain.Entities;

public class Deporte
{
    public long Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Nombre { get; set; } = null!;

    [MaxLength(200)]
    public string? Icon { get; set; }

    [MaxLength(100)]
    public string? BgClass { get; set; }

    [MaxLength(100)]
    public string? TextClass { get; set; }

    // Navegación inversa
    public ICollection<Complejo> Complejos { get; set; } = new List<Complejo>();
}
