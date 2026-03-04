namespace LaReservaBackend.Application.Reservas.Queries;

public class GetUserReservationsResponse
{
    public List<ReservaDetalleResponse> PartidosPendientes { get; set; } = new();
    public List<ReservaDetalleResponse> TurnosAntiguos { get; set; } = new();
}

public class ReservaDetalleResponse
{
    public long Id { get; set; }
    public string Complejo { get; set; } = string.Empty;
    public string Cancha { get; set; } = string.Empty;
    public string Deporte { get; set; } = string.Empty;
    public string Fecha { get; set; } = string.Empty;
    public string Hora { get; set; } = string.Empty;
    public string HoraFin { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
    public string MedioPago { get; set; } = string.Empty;
    public bool RequiereComprobante { get; set; }
    public decimal Precio { get; set; }
    public bool Confirmada { get; set; }
}
