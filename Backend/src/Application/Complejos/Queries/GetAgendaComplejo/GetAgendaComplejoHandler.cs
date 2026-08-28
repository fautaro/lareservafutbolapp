using LaReservaBackend.Application.Common.Exceptions;
using LaReservaBackend.Application.Common.Interfaces;
using LaReservaBackend.Domain.Entities;
using LaReservaBackend.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LaReservaBackend.Application.Complejos.Queries.GetAgendaComplejo;

public class GetAgendaComplejoHandler : IRequestHandler<GetAgendaComplejoQuery, OwnerAgendaResponse>
{
    private readonly IApplicationDbContext _context;

    public GetAgendaComplejoHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<OwnerAgendaResponse> Handle(GetAgendaComplejoQuery request, CancellationToken cancellationToken)
    {
        var usuario = await _context.Usuarios
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == request.UsuarioId, cancellationToken);

        if (usuario == null)
        {
            throw new UnauthorizedAccessException();
        }

        var complejo = await _context.Complejos
            .Include(c => c.Canchas)
                .ThenInclude(ca => ca.TipoCancha)
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == request.ComplejoId, cancellationToken);

        if (complejo == null)
        {
            throw new KeyNotFoundException($"Complejo {request.ComplejoId} not found.");
        }

        if (complejo.DuenoId != usuario.Id)
        {
            throw new ForbiddenAccessException();
        }

        var dateStart = request.Fecha.Date;
        var dateEnd = dateStart.AddDays(1);

        int diaSemanaDb = request.Fecha.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)request.Fecha.DayOfWeek;

        var canchasIds = complejo.Canchas.Select(c => c.Id).ToList();

        var horariosCanchas = await _context.HorariosCanchas
            .AsNoTracking()
            .Where(hc => canchasIds.Contains(hc.CanchaId) && hc.DiaSemana == diaSemanaDb && hc.Disponible)
            .ToListAsync(cancellationToken);

        var reservas = await _context.Reservas
            .Include(r => r.Usuario)
            .Include(r => r.MedioPago)
            .AsNoTracking()
            .Where(r => r.ComplejoId == request.ComplejoId 
                        && r.Fecha >= dateStart 
                        && r.Fecha < dateEnd
                        && r.Estado != EstadoReserva.Eliminado)
            .ToListAsync(cancellationToken);

        var response = new OwnerAgendaResponse
        {
            ComplejoId = complejo.Id,
            ComplejoNombre = complejo.Nombre,
            Fecha = dateStart
        };

        foreach (var cancha in complejo.Canchas.OrderBy(c => c.Nombre))
        {
            var agendaCancha = new AgendaCanchaDto
            {
                CanchaId = cancha.Id,
                CanchaNombre = cancha.Nombre,
                DeporteNombre = cancha.TipoCancha?.Nombre ?? string.Empty
            };

            var horariosParaCancha = horariosCanchas.Where(hc => hc.CanchaId == cancha.Id).ToList();
            var reservasParaCancha = reservas.Where(r => r.CanchaId == cancha.Id).ToList();

            if (request.SoloReservas)
            {
                foreach (var reserva in reservasParaCancha.OrderBy(r => r.Fecha))
                {
                    var turno = new TurnoAgendaDto
                    {
                        HoraInicio = reserva.Fecha.TimeOfDay,
                        HoraFin = reserva.FechaFin.TimeOfDay,
                        Estado = "reserved",
                        ReservaId = reserva.Id,
                        JugadorNombre = reserva.Usuario?.Nombre,
                        JugadorTelefono = reserva.Usuario?.Telefono,
                        JugadorEmail = reserva.Usuario?.Email,
                        MedioPagoNombre = reserva.MedioPago?.Nombre,
                        MontoTotal = reserva.MontoTotal,
                        EstadoPago = reserva.EstadoPago.ToString(),
                        Confirmada = reserva.Confirmada,
                        EstadoReserva = reserva.Estado.ToString(),
                        FechaReserva = reserva.FechaReserva
                    };
                    agendaCancha.Turnos.Add(turno);
                }
            }
            else
            {
                foreach (var hc in horariosParaCancha)
                {
                    var currentTime = hc.HoraInicio;
                    // Assuming slots of 1 hour as standard, or we use the specific times
                    // If HoraInicio=08:00 and HoraFin=23:00, we should generate hourly slots.
                    // Let's iterate from HoraInicio to HoraFin by 1 hour increments.
                    while (currentTime < hc.HoraFin)
                    {
                        var nextTime = currentTime + TimeSpan.FromHours(1);
                        
                        var reserva = reservasParaCancha.FirstOrDefault(r => 
                            r.Fecha.TimeOfDay < nextTime && r.FechaFin.TimeOfDay > currentTime);

                        var turno = new TurnoAgendaDto
                        {
                            HoraInicio = currentTime,
                            HoraFin = nextTime,
                            Estado = reserva == null ? "available" : (reserva.Estado == EstadoReserva.Bloqueado ? "blocked" : "reserved"),
                            ReservaId = reserva?.Id,
                            JugadorNombre = reserva?.Usuario?.Nombre,
                            JugadorTelefono = reserva?.Usuario?.Telefono,
                            JugadorEmail = reserva?.Usuario?.Email,
                            MedioPagoNombre = reserva?.MedioPago?.Nombre,
                            MontoTotal = reserva?.MontoTotal,
                            EstadoPago = reserva?.EstadoPago.ToString(),
                            Confirmada = reserva?.Confirmada,
                            EstadoReserva = reserva?.Estado.ToString(),
                            FechaReserva = reserva?.FechaReserva
                        };

                        agendaCancha.Turnos.Add(turno);
                        currentTime = nextTime;
                    }
                }
            }

            response.Canchas.Add(agendaCancha);
        }

        return response;
    }
}
