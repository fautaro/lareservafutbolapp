using LaReservaBackend.Application.Common.Interfaces;
using LaReservaBackend.Application.Reservas.Queries;
using LaReservaBackend.Domain.Enums;
using LaReservaBackend.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LaReservaBackend.Infrastructure.Repositories.Reserva;
public class ReservaRepository : IReservaRepository
{
    private readonly IApplicationDbContext _context;

    public ReservaRepository(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<GetHorariosDisponiblesResponse> GetHorariosDisponibles(long complejoId, CancellationToken cancellationToken = default)
    {
        // Obtener el complejo con sus canchas
        var complejo = await _context.Complejos
            .Include(c => c.Canchas)
            .FirstOrDefaultAsync(c => c.Id == complejoId, cancellationToken);

        if (complejo == null)
        {
            return new GetHorariosDisponiblesResponse();
        }

        var maxDias = complejo.MaxDiasDisponiblesReserva ?? 7; 
        var canchasIds = complejo.Canchas.Select(c => c.Id).ToList();
        var fechaInicio = DateTime.SpecifyKind(DateTime.Today, DateTimeKind.Unspecified);
        var fechaFin = fechaInicio.AddDays(maxDias);

        // Obtener todos los horarios configurados para las canchas del complejo
        var horariosCanchas = await _context.HorariosCanchas
            .Include(h => h.Cancha!)
                .ThenInclude(c => c.TipoCancha!)
            .Where(h => canchasIds.Contains(h.CanchaId) && h.Disponible)
            .ToListAsync(cancellationToken);

        // Obtener todas las reservas confirmadas de todas las canchas del complejo
        var reservasConfirmadas = await _context.Reservas
            .Where(r => canchasIds.Contains(r.CanchaId) 
                && r.Confirmada 
                && r.Fecha >= fechaInicio 
                && r.Fecha < fechaFin)
            .Select(r => new { r.CanchaId, r.Fecha, r.FechaFin })
            .ToListAsync(cancellationToken);

        var horariosPorDia = new List<HorarioPorDia>();

        for (int i = 0; i < maxDias; i++)
        {
            var fechaActual = fechaInicio.AddDays(i);
            var diaSemana = ObtenerDiaSemana(fechaActual.DayOfWeek);

            var horariosDisponibles = new List<HorarioDisponible>();

        foreach (var horario in horariosCanchas.Where(h => h.DiaSemana == diaSemana))
        {
            var fechaHoraInicio = fechaActual.Date + horario.HoraInicio;
            var fechaHoraFin = fechaActual.Date + horario.HoraFin;

            // Verificar si el horario ya pasó (si es hoy)
            if (fechaActual.Date == DateTime.Today && fechaHoraInicio <= DateTime.Now)
            {
                continue;
            }

            // Verificar si este horario específico de esta cancha está libre
            var estaReservado = reservasConfirmadas.Any(r => 
                r.CanchaId == horario.CanchaId && 
                ((fechaHoraInicio >= r.Fecha && fechaHoraInicio < r.FechaFin) ||
                 (fechaHoraFin > r.Fecha && fechaHoraFin <= r.FechaFin) ||
                 (fechaHoraInicio <= r.Fecha && fechaHoraFin >= r.FechaFin))
            );

            if (!estaReservado)
            {
                horariosDisponibles.Add(new HorarioDisponible
                {
                    HorarioCanchaId = horario.Id,
                    CanchaId = horario.CanchaId,
                    CanchaNombre = horario.Cancha?.Nombre ?? "Cancha",
                    PrecioHora = horario.Cancha?.PrecioHora ?? 0,
                    HoraInicio = horario.HoraInicio,
                    HoraFin = horario.HoraFin,
                    TipoCancha = horario.Cancha?.TipoCancha?.Nombre ?? "Deporte"
                });
            }
        }

        if (horariosDisponibles.Any())
        {
            horariosPorDia.Add(new HorarioPorDia
            {
                Fecha = fechaActual,
                DiaSemana = diaSemana,
                Horarios = horariosDisponibles.OrderBy(h => h.CanchaNombre).ThenBy(h => h.HoraInicio).ToList()
            });
        }    }
        

        return new GetHorariosDisponiblesResponse
        {
            Complejo = new ComplejoDetalleResponse
            {
                Id = complejo.Id,
                Nombre = complejo.Nombre,
                Direccion = complejo.Direccion ?? string.Empty,
                Imagen = complejo.Imagen
            },
            HorariosPorDia = horariosPorDia
        };
    }

    public async Task<GetUserReservationsResponse> GetUserReservations(long usuarioId, CancellationToken cancellationToken = default)
    {
        var now = DateTime.Now;

        var allReservas = await _context.Reservas
            .Include(r => r.Complejo!)
            .Include(r => r.Cancha!)
                .ThenInclude(c => c.TipoCancha!)
            .Include(r => r.MedioPago!)
            .Where(r => r.UsuarioId == usuarioId && r.Estado == EstadoReserva.Confirmado)
            .OrderByDescending(r => r.Fecha)
            .ToListAsync(cancellationToken);

        var pendientes = allReservas
            .Where(r => r.FechaFin >= now)
            .Select(r => new ReservaDetalleResponse
            {
                Id = r.Id,
                Complejo = r.Complejo != null ? r.Complejo.Nombre : "Complejo",
                Cancha = r.Cancha != null ? r.Cancha.Nombre : "Cancha",
                Deporte = (r.Cancha != null && r.Cancha.TipoCancha != null) ? r.Cancha.TipoCancha.Nombre : "Deporte",
                Fecha = r.Fecha.ToString("dd/MM/yyyy"),
                Hora = r.Fecha.ToString("HH:mm"),
                HoraFin = r.FechaFin.ToString("HH:mm"),
                Estado = r.Confirmada ? "Confirmado" : "Pendiente",
                MedioPago = r.MedioPago != null ? r.MedioPago.Nombre : "No especificado",
                RequiereComprobante = r.MedioPago != null && r.MedioPago.RequiereComprobante,
                Precio = r.MontoTotal ?? 0,
                Confirmada = r.Confirmada
            })
            .ToList();

        var antiguos = allReservas
            .Where(r => r.FechaFin < now)
            .Select(r => new ReservaDetalleResponse
            {
                Id = r.Id,
                Complejo = r.Complejo != null ? r.Complejo.Nombre : "Complejo",
                Cancha = r.Cancha != null ? r.Cancha.Nombre : "Cancha",
                Deporte = (r.Cancha != null && r.Cancha.TipoCancha != null) ? r.Cancha.TipoCancha.Nombre : "Deporte",
                Fecha = r.Fecha.ToString("dd/MM/yyyy"),
                Hora = r.Fecha.ToString("HH:mm"),
                HoraFin = r.FechaFin.ToString("HH:mm"),
                Estado = "Finalizado",
                MedioPago = r.MedioPago != null ? r.MedioPago.Nombre : "No especificado",
                RequiereComprobante = r.MedioPago != null && r.MedioPago.RequiereComprobante,
                Precio = r.MontoTotal ?? 0,
                Confirmada = r.Confirmada
            })
            .ToList();

        return new GetUserReservationsResponse
        {
            PartidosPendientes = pendientes,
            TurnosAntiguos = antiguos
        };
    }

    public async Task<bool> CancelReservation(long reservaId, CancellationToken cancellationToken = default)
    {
        var reserva = await _context.Reservas.FindAsync(new object[] { reservaId }, cancellationToken);
        if (reserva == null) return false;

        reserva.Estado = EstadoReserva.Eliminado;
        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<long> CreateReservation(LaReservaBackend.Domain.Entities.Reserva reserva, CancellationToken cancellationToken = default)
    {
        // Verificar disponibilidad de último momento
        var existeSobreposicion = await _context.Reservas
            .AnyAsync(r => 
                r.CanchaId == reserva.CanchaId &&
                r.Estado != EstadoReserva.Eliminado &&
                ((reserva.Fecha >= r.Fecha && reserva.Fecha < r.FechaFin) ||
                 (reserva.FechaFin > r.Fecha && reserva.FechaFin <= r.FechaFin) ||
                 (reserva.Fecha <= r.Fecha && reserva.FechaFin >= r.FechaFin))
            , cancellationToken);

        if (existeSobreposicion) return -1;

        _context.Reservas.Add(reserva);
        await _context.SaveChangesAsync(cancellationToken);
        return reserva.Id;
    }

    public async Task<ReservaDetalleResponse?> GetNextReserva(long usuarioId, CancellationToken cancellationToken = default)
    {
        var now = DateTime.Now;
        var proxima = await _context.Reservas
            .Include(r => r.Complejo!)
            .Include(r => r.Cancha!)
                .ThenInclude(c => c.TipoCancha!)
            .Include(r => r.MedioPago!)
            .Where(r => r.UsuarioId == usuarioId && r.Estado == EstadoReserva.Confirmado && r.Fecha >= now)
            .OrderBy(r => r.Fecha)
            .Select(r => new ReservaDetalleResponse
            {
                Id = r.Id,
                Complejo = r.Complejo != null ? r.Complejo.Nombre : "Complejo",
                Cancha = r.Cancha != null ? r.Cancha.Nombre : "Cancha",
                Deporte = (r.Cancha != null && r.Cancha.TipoCancha != null) ? r.Cancha.TipoCancha.Nombre : "Deporte",
                Fecha = r.Fecha.ToString("dd/MM/yyyy"),
                Hora = r.Fecha.ToString("HH:mm"),
                HoraFin = r.FechaFin.ToString("HH:mm"),
                Estado = r.Confirmada ? "Confirmado" : "Pendiente",
                MedioPago = r.MedioPago != null ? r.MedioPago.Nombre : "No especificado",
                RequiereComprobante = r.MedioPago != null && r.MedioPago.RequiereComprobante,
                Precio = r.MontoTotal ?? 0,
                Confirmada = r.Confirmada
            })
            .FirstOrDefaultAsync(cancellationToken);

        return proxima;
    }

    private int ObtenerDiaSemana(DayOfWeek dayOfWeek)
    {
        // Convertir DayOfWeek de .NET (Sunday=0) a formato base de datos (Monday=1)
        return dayOfWeek switch
        {
            DayOfWeek.Monday => 1,
            DayOfWeek.Tuesday => 2,
            DayOfWeek.Wednesday => 3,
            DayOfWeek.Thursday => 4,
            DayOfWeek.Friday => 5,
            DayOfWeek.Saturday => 6,
            DayOfWeek.Sunday => 7,
            _ => 1
        };
    }
}
