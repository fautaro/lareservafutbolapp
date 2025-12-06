using System.ComponentModel.DataAnnotations;

namespace LaReservaBackend.Domain.Entities;
public class Ciudad
{
    public long Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Nombre { get; set; } = null!;

    // Navegación
    public ICollection<Complejo> Complejos { get; set; } = new List<Complejo>();
}
