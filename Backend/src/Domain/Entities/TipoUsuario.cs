using System.ComponentModel.DataAnnotations;

namespace LaReservaBackend.Domain.Entities;
public class TipoUsuario
{
    public long Id { get; set; }

    [Required]
    [MaxLength(20)]
    public string Nombre { get; set; } = null!;

    // Navegación
    public ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
