using System.ComponentModel.DataAnnotations;

namespace LaReservaBackend.Domain.Entities;
public class Usuario
{
    public long Id { get; set; }

    [MaxLength(128)]
    public string? Auth0Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Nombre { get; set; } = null!;

    [MaxLength(100)]
    public string? Apellido { get; set; }

    [MaxLength(20)]
    public string? Dni { get; set; }

    [Required]
    [MaxLength(100)]
    public string Email { get; set; } = null!;

    [MaxLength(20)]
    public string? Telefono { get; set; }

    public bool Activo { get; set; } = true;

    [MaxLength(255)]
    public string? PasswordHash { get; set; }

    public bool DebeCambiarPassword { get; set; } = false;

    public long TipoUsuarioId { get; set; }
    public TipoUsuario? TipoUsuario { get; set; }

    public DateTime FechaRegistro { get; set; }

    // Navegaciones
    public ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
    public ICollection<Complejo> ComplejosComoDueno { get; set; } = new List<Complejo>();
}
