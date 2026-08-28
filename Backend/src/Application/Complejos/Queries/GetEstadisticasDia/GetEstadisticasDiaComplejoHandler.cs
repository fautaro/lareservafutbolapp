using LaReservaBackend.Application.Common.Exceptions;
using LaReservaBackend.Application.Common.Interfaces;
using LaReservaBackend.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LaReservaBackend.Application.Complejos.Queries.GetEstadisticasDia;

public class GetEstadisticasDiaComplejoHandler : IRequestHandler<GetEstadisticasDiaComplejoQuery, EstadisticasDiaDto>
{
    private readonly IApplicationDbContext _context;

    public GetEstadisticasDiaComplejoHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<EstadisticasDiaDto> Handle(GetEstadisticasDiaComplejoQuery request, CancellationToken cancellationToken)
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

        return EstadisticasComplejoHelper.CalcularEstadisticas(complejo, horariosCanchas, reservas);
    }
}
