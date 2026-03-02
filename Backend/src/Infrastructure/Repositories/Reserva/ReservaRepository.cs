using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaReservaBackend.Application.Common.Interfaces;
using LaReservaBackend.Application.Reservas.Queries;
using LaReservaBackend.Domain.Enums;
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
        var fechaInicio = DateTime.SpecifyKind(DateTime.Today, DateTimeKind.Utc);
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

            // Agrupar horarios por rango de tiempo y TIPO DE CANCHA para el día de la semana
            var slotsDelDia = horariosCanchas
                .Where(h => h.DiaSemana == diaSemana)
                .GroupBy(h => new { 
                    h.HoraInicio, 
                    h.HoraFin, 
                    TipoCancha = h.Cancha?.TipoCancha?.Nombre ?? "Deporte" 
                })
                .ToList();

            var horariosDisponibles = new List<HorarioDisponible>();

            foreach (var slot in slotsDelDia)
            {
                var fechaHoraInicio = fechaActual.Date + slot.Key.HoraInicio;
                var fechaHoraFin = fechaActual.Date + slot.Key.HoraFin;

                // Verificar si al menos una cancha configurada para este horario y deporte está libre
                var horarioLibre = slot.FirstOrDefault(horario => 
                    !reservasConfirmadas.Any(r => 
                        r.CanchaId == horario.CanchaId && 
                        ((fechaHoraInicio >= r.Fecha && fechaHoraInicio < r.FechaFin) ||
                         (fechaHoraFin > r.Fecha && fechaHoraFin <= r.FechaFin) ||
                         (fechaHoraInicio <= r.Fecha && fechaHoraFin >= r.FechaFin))
                    )
                );

                if (horarioLibre != null)
                {
                    horariosDisponibles.Add(new HorarioDisponible
                    {
                        HorarioCanchaId = horarioLibre.Id,
                        HoraInicio = horarioLibre.HoraInicio,
                        HoraFin = horarioLibre.HoraFin,
                        TipoCancha = slot.Key.TipoCancha
                    });
                }
            }

            if (horariosDisponibles.Any())
            {
                horariosPorDia.Add(new HorarioPorDia
                {
                    Fecha = fechaActual,
                    DiaSemana = diaSemana,
                    Horarios = horariosDisponibles.OrderBy(h => h.HoraInicio).ToList()
                });
            }
        }

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
        var now = DateTime.UtcNow;

        var allReservas = await _context.Reservas
            .Include(r => r.Complejo!)
            .Include(r => r.Cancha!)
                .ThenInclude(c => c.TipoCancha!)
            .Where(r => r.UsuarioId == usuarioId && r.Estado == EstadoReserva.Confirmado)
            .OrderByDescending(r => r.Fecha)
            .ToListAsync(cancellationToken);

        var pendientes = allReservas
            .Where(r => r.Fecha >= now)
            .Select(r => new ReservaDetalleResponse
            {
                Id = r.Id,
                Complejo = r.Complejo?.Nombre ?? "Complejo",
                Cancha = r.Cancha?.Nombre ?? "Cancha",
                Deporte = r.Cancha?.TipoCancha?.Nombre ?? "Deporte",
                Fecha = r.Fecha.ToString("dd/MM/yyyy"),
                Hora = r.Fecha.ToString("HH:mm"),
                Estado = r.Confirmada ? "Confirmado" : "Pendiente",
                Confirmada = r.Confirmada
            })
            .ToList();

        var antiguos = allReservas
            .Where(r => r.Fecha < now)
            .Select(r => new ReservaDetalleResponse
            {
                Id = r.Id,
                Complejo = r.Complejo?.Nombre ?? "Complejo",
                Cancha = r.Cancha?.Nombre ?? "Cancha",
                Deporte = r.Cancha?.TipoCancha?.Nombre ?? "Deporte",
                Fecha = r.Fecha.ToString("dd/MM/yyyy"),
                Hora = r.Fecha.ToString("HH:mm"),
                Estado = "Finalizado",
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
