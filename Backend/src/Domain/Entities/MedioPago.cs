using System.ComponentModel.DataAnnotations;
using LaReservaBackend.Domain.Common;

namespace LaReservaBackend.Domain.Entities;

public class MedioPago
{
    public int Id { get; set; }

    [MaxLength(50)]
    public string Nombre { get; set; } = null!;

    [MaxLength(20)]
    public string Codigo { get; set; } = null!;

    public string? Descripcion { get; set; }

    [MaxLength(50)]
    public string? Icono { get; set; }

    public int Orden { get; set; } = 0;

    public bool Activo { get; set; } = true;

    public bool RequiereComprobante { get; set; } = false;

    public DateTime FechaCreacion { get; set; } = DateTime.Now;

    // Relación con Reservas
    public ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
}
