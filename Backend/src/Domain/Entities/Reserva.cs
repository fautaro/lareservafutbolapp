using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LaReservaBackend.Domain.Enums;

namespace LaReservaBackend.Domain.Entities;
public class Reserva
{
    public long Id { get; set; }

    public long UsuarioId { get; set; }
    public Usuario? Usuario { get; set; }

    public long ComplejoId { get; set; }
    public Complejo? Complejo { get; set; }

    public long CanchaId { get; set; }
    public Cancha? Cancha { get; set; }

    public DateTime Fecha { get; set; }

    public DateTime FechaFin { get; set; }

    [MaxLength(50)]
    public string? MedioPago { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal? MontoTotal { get; set; }

    public EstadoPago EstadoPago { get; set; } = EstadoPago.pendiente;

    public bool Confirmada { get; set; } = false;

    public EstadoReserva Estado { get; set; } = EstadoReserva.Pendiente;

    public DateTime FechaReserva { get; set; }
}
