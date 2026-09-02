using LaReservaBackend.Application.Common.Exceptions;
using LaReservaBackend.Application.Common.Interfaces;
using LaReservaBackend.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LaReservaBackend.Application.Reservas.Commands;

public class ConfirmReservationCommand : IRequest<bool>
{
    public long ReservaId { get; set; }
    public long UsuarioId { get; set; }
}

public class ConfirmReservationHandler : IRequestHandler<ConfirmReservationCommand, bool>
{
    private readonly IApplicationDbContext _context;

    public ConfirmReservationHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(ConfirmReservationCommand request, CancellationToken cancellationToken)
    {
        var reserva = await _context.Reservas
            .Include(r => r.Complejo)
            .FirstOrDefaultAsync(r => r.Id == request.ReservaId, cancellationToken);

        if (reserva == null)
        {
            throw new KeyNotFoundException($"Reserva {request.ReservaId} no encontrada.");
        }

        if (reserva.Complejo == null || reserva.Complejo.DuenoId != request.UsuarioId)
        {
            throw new ForbiddenAccessException();
        }

        reserva.Estado = EstadoReserva.Confirmado;
        reserva.Confirmada = true;
        
        await _context.SaveChangesAsync(cancellationToken);
        
        return true;
    }
}
