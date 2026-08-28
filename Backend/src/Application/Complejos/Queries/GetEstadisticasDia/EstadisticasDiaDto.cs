namespace LaReservaBackend.Application.Complejos.Queries.GetEstadisticasDia;

public class EstadisticasDiaDto
{
    public decimal GananciasConfirmadas { get; set; }
    public decimal GananciasCobradas { get; set; }
    public decimal GananciasPendientesCobro { get; set; }
    public int CantidadReservasTotales { get; set; }
    public int CantidadReservasConfirmadas { get; set; }
    public int CantidadReservasPendientes { get; set; }
    public int CantidadHorariosBloqueados { get; set; }
    public int CantidadHorariosDisponibles { get; set; }
    public int TotalTurnosDisponibles { get; set; }
    public double PorcentajeOcupacion { get; set; }
    public List<EstadisticaCanchaDto> DesgloseCanchas { get; set; } = new();
    public List<EstadisticaMedioPagoDto> DesgloseMediosPago { get; set; } = new();
}

public class EstadisticaCanchaDto
{
    public long CanchaId { get; set; }
    public string CanchaNombre { get; set; } = string.Empty;
    public string DeporteNombre { get; set; } = string.Empty;
    public int ReservasCount { get; set; }
    public int TotalTurnos { get; set; }
    public double PorcentajeOcupacion { get; set; }
    public decimal Ingresos { get; set; }
}

public class EstadisticaMedioPagoDto
{
    public string MedioPagoNombre { get; set; } = string.Empty;
    public int CantidadReservas { get; set; }
    public decimal MontoTotal { get; set; }
}
