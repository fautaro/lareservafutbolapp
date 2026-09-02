using LaReservaBackend.Application.Common.Exceptions;
using LaReservaBackend.Application.Common.Interfaces;
using LaReservaBackend.Application.Complejos.Queries.GetEstadisticasDia;
using LaReservaBackend.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LaReservaBackend.Application.Complejos.Queries.GetEstadisticas;

public class GetEstadisticasHandler : IRequestHandler<GetEstadisticasQuery, EstadisticasDiaDto>
{
    private readonly IApplicationDbContext _context;

    public GetEstadisticasHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<EstadisticasDiaDto> Handle(GetEstadisticasQuery request, CancellationToken cancellationToken)
    {
        var usuario = await _context.Usuarios
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == request.UsuarioId, cancellationToken);

        if (usuario == null)
        {
            throw new UnauthorizedAccessException();
        }

        var complejosQuery = _context.Complejos
            .Include(c => c.Canchas)
                .ThenInclude(ca => ca.TipoCancha)
            .AsNoTracking()
            .Where(c => c.DuenoId == usuario.Id);

        if (request.ComplejoId.HasValue && request.ComplejoId.Value > 0)
        {
            complejosQuery = complejosQuery.Where(c => c.Id == request.ComplejoId.Value);
        }

        var complejos = await complejosQuery.ToListAsync(cancellationToken);

        if (!complejos.Any())
        {
            if (request.ComplejoId.HasValue && request.ComplejoId.Value > 0)
            {
                throw new KeyNotFoundException($"Complejo {request.ComplejoId} not found or access denied.");
            }
            return new EstadisticasDiaDto(); // Empty stats
        }

        var complejosIds = complejos.Select(c => c.Id).ToList();
        var canchasIds = complejos.SelectMany(c => c.Canchas.Select(ca => ca.Id)).ToList();

        var dateStart = request.FechaInicio.Date;
        var dateEnd = request.FechaFin.Date.AddDays(1);

        // Compute total days per DayOfWeek in the range
        var daysCount = new Dictionary<int, int>();
        for (int i = 1; i <= 7; i++) daysCount[i] = 0;

        for (var date = dateStart; date < dateEnd; date = date.AddDays(1))
        {
            int dow = date.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)date.DayOfWeek;
            daysCount[dow]++;
        }

        var horariosCanchas = await _context.HorariosCanchas
            .AsNoTracking()
            .Where(hc => canchasIds.Contains(hc.CanchaId) && hc.Disponible)
            .ToListAsync(cancellationToken);

        var reservas = await _context.Reservas
            .Include(r => r.Usuario)
            .Include(r => r.MedioPago)
            .AsNoTracking()
            .Where(r => complejosIds.Contains(r.ComplejoId) 
                        && r.Fecha >= dateStart 
                        && r.Fecha < dateEnd
                        && r.Estado != EstadoReserva.Eliminado)
            .ToListAsync(cancellationToken);

        var activas = reservas.Where(r => r.Estado != EstadoReserva.Eliminado && r.Estado != EstadoReserva.Bloqueado).ToList();
        var confirmadas = activas.Where(r => r.Estado == EstadoReserva.Confirmado || r.Confirmada).ToList();
        var pendientes = activas.Where(r => r.Estado == EstadoReserva.Pendiente && !r.Confirmada).ToList();
        var bloqueadas = reservas.Where(r => r.Estado == EstadoReserva.Bloqueado).ToList();

        decimal gananciasConfirmadas = confirmadas.Sum(r => r.MontoTotal ?? 0);
        decimal gananciasCobradas = confirmadas.Where(r => r.EstadoPago == EstadoPago.pagado).Sum(r => r.MontoTotal ?? 0);
        decimal gananciasPendientesCobro = confirmadas.Where(r => r.EstadoPago == EstadoPago.pendiente).Sum(r => r.MontoTotal ?? 0);

        int totalTurnosGeneral = 0;
        var desgloseCanchas = new List<EstadisticaCanchaDto>();

        foreach (var complejo in complejos)
        {
            foreach (var cancha in complejo.Canchas.OrderBy(c => c.Nombre))
            {
                var horariosCancha = horariosCanchas.Where(hc => hc.CanchaId == cancha.Id).ToList();
                int totalTurnosCancha = 0;

                foreach (var hc in horariosCancha)
                {
                    var diff = (hc.HoraFin - hc.HoraInicio).TotalHours;
                    if (diff > 0)
                    {
                        int occurrences = daysCount[hc.DiaSemana];
                        totalTurnosCancha += (int)diff * occurrences;
                    }
                }

                totalTurnosGeneral += totalTurnosCancha;

                var reservasCancha = activas.Where(r => r.CanchaId == cancha.Id).ToList();
                var confirmadasCancha = confirmadas.Where(r => r.CanchaId == cancha.Id).ToList();

                double ocupacionCancha = totalTurnosCancha > 0 
                    ? Math.Round((double)reservasCancha.Count / totalTurnosCancha * 100.0, 1) 
                    : 0;

                desgloseCanchas.Add(new EstadisticaCanchaDto
                {
                    CanchaId = cancha.Id,
                    CanchaNombre = cancha.Nombre,
                    ComplejoNombre = complejo.Nombre,
                    DeporteNombre = cancha.TipoCancha?.Nombre ?? string.Empty,
                    ReservasCount = reservasCancha.Count,
                    TotalTurnos = totalTurnosCancha,
                    PorcentajeOcupacion = Math.Min(100.0, ocupacionCancha),
                    Ingresos = confirmadasCancha.Sum(r => r.MontoTotal ?? 0)
                });
            }
        }

        int cantidadDisponibles = Math.Max(0, totalTurnosGeneral - activas.Count - bloqueadas.Count);
        double ocupacionGeneral = totalTurnosGeneral > 0 
            ? Math.Round((double)activas.Count / totalTurnosGeneral * 100.0, 1) 
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

        var desgloseHorarios = activas
            .GroupBy(r => r.Fecha.ToString("HH:mm"))
            .Select(g => new EstadisticaHorarioDto
            {
                Horario = g.Key,
                CantidadReservas = g.Count()
            })
            .OrderBy(h => h.Horario)
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
            TotalTurnosDisponibles = totalTurnosGeneral,
            PorcentajeOcupacion = Math.Min(100.0, ocupacionGeneral),
            DesgloseCanchas = desgloseCanchas,
            DesgloseMediosPago = desgloseMediosPago,
            DesgloseHorarios = desgloseHorarios
        };
    }
}
