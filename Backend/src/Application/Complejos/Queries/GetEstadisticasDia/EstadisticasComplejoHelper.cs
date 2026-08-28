using LaReservaBackend.Domain.Entities;
using LaReservaBackend.Domain.Enums;

namespace LaReservaBackend.Application.Complejos.Queries.GetEstadisticasDia;

public static class EstadisticasComplejoHelper
{
    public static EstadisticasDiaDto CalcularEstadisticas(Complejo complejo, List<HorarioCancha> horariosCanchas, List<Reserva> reservas)
    {
        var activas = reservas.Where(r => r.Estado != EstadoReserva.Eliminado && r.Estado != EstadoReserva.Bloqueado).ToList();
        var confirmadas = activas.Where(r => r.Estado == EstadoReserva.Confirmado || r.Confirmada).ToList();
        var pendientes = activas.Where(r => r.Estado == EstadoReserva.Pendiente && !r.Confirmada).ToList();
        var bloqueadas = reservas.Where(r => r.Estado == EstadoReserva.Bloqueado).ToList();

        decimal gananciasConfirmadas = confirmadas.Sum(r => r.MontoTotal ?? 0);
        decimal gananciasCobradas = confirmadas.Where(r => r.EstadoPago == EstadoPago.pagado).Sum(r => r.MontoTotal ?? 0);
        decimal gananciasPendientesCobro = confirmadas.Where(r => r.EstadoPago == EstadoPago.pendiente).Sum(r => r.MontoTotal ?? 0);

        int totalTurnosComplejo = 0;
        var desgloseCanchas = new List<EstadisticaCanchaDto>();

        foreach (var cancha in complejo.Canchas.OrderBy(c => c.Nombre))
        {
            var horariosCancha = horariosCanchas.Where(hc => hc.CanchaId == cancha.Id).ToList();
            int totalTurnosCancha = 0;
            foreach (var hc in horariosCancha)
            {
                var diff = (hc.HoraFin - hc.HoraInicio).TotalHours;
                if (diff > 0)
                {
                    totalTurnosCancha += (int)diff;
                }
            }

            totalTurnosComplejo += totalTurnosCancha;

            var reservasCancha = activas.Where(r => r.CanchaId == cancha.Id).ToList();
            var confirmadasCancha = confirmadas.Where(r => r.CanchaId == cancha.Id).ToList();

            double ocupacionCancha = totalTurnosCancha > 0 
                ? Math.Round((double)reservasCancha.Count / totalTurnosCancha * 100.0, 1) 
                : 0;

            desgloseCanchas.Add(new EstadisticaCanchaDto
            {
                CanchaId = cancha.Id,
                CanchaNombre = cancha.Nombre,
                DeporteNombre = cancha.TipoCancha?.Nombre ?? string.Empty,
                ReservasCount = reservasCancha.Count,
                TotalTurnos = totalTurnosCancha,
                PorcentajeOcupacion = Math.Min(100.0, ocupacionCancha),
                Ingresos = confirmadasCancha.Sum(r => r.MontoTotal ?? 0)
            });
        }

        int cantidadDisponibles = Math.Max(0, totalTurnosComplejo - activas.Count - bloqueadas.Count);
        double ocupacionGeneral = totalTurnosComplejo > 0 
            ? Math.Round((double)activas.Count / totalTurnosComplejo * 100.0, 1) 
            : 0;

        var desgloseMediosPago = confirmadas
            .GroupBy(r => !string.IsNullOrWhiteSpace(r.MedioPago?.Nombre) ? r.MedioPago.Nombre : "Efectivo / En complejo")
            .Select(g => new EstadisticaMedioPagoDto
            {
                MedioPagoNombre = g.Key,
                CantidadReservas = g.Count(),
                MontoTotal = g.Sum(r => r.MontoTotal ?? 0)
            })
            .OrderByDescending(m => m.MontoTotal)
            .ToList();

        return new EstadisticasDiaDto
        {
            GananciasConfirmadas = gananciasConfirmadas,
            GananciasCobradas = gananciasCobradas,
            GananciasPendientesCobro = gananciasPendientesCobro,
            CantidadReservasTotales = activas.Count,
            CantidadReservasConfirmadas = confirmadas.Count,
            CantidadReservasPendientes = pendientes.Count,
            CantidadHorariosBloqueados = bloqueadas.Count,
            CantidadHorariosDisponibles = cantidadDisponibles,
            TotalTurnosDisponibles = totalTurnosComplejo,
            PorcentajeOcupacion = Math.Min(100.0, ocupacionGeneral),
            DesgloseCanchas = desgloseCanchas,
            DesgloseMediosPago = desgloseMediosPago
        };
    }
}
