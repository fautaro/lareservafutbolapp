namespace LaReservaBackend.Application.Complejos.Queries.GetAgendaComplejo;

public class OwnerAgendaResponse
{
    public long ComplejoId { get; set; }
    public string ComplejoNombre { get; set; } = string.Empty;
    public DateTime Fecha { get; set; }
    public List<AgendaCanchaDto> Canchas { get; set; } = new();
}

public class AgendaCanchaDto
{
    public long CanchaId { get; set; }
    public string CanchaNombre { get; set; } = string.Empty;
    public string DeporteNombre { get; set; } = string.Empty;
    public List<TurnoAgendaDto> Turnos { get; set; } = new();
}

public class TurnoAgendaDto
{
    public TimeSpan HoraInicio { get; set; }
    public TimeSpan HoraFin { get; set; }
    public string Estado { get; set; } = string.Empty; // "available" or "reserved"
    public long? ReservaId { get; set; }
    public string? JugadorNombre { get; set; }
    public string? JugadorTelefono { get; set; }
    public string? JugadorEmail { get; set; }
    public string? MedioPagoNombre { get; set; }
    public decimal? MontoTotal { get; set; }
    public string? EstadoPago { get; set; }
    public bool? Confirmada { get; set; }
    public string? EstadoReserva { get; set; }
    public DateTime? FechaReserva { get; set; }
}
