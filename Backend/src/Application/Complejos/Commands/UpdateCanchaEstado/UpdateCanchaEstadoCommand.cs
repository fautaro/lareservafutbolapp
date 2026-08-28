using MediatR;
using LaReservaBackend.Application.Common.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace LaReservaBackend.Application.Complejos.Commands.UpdateCanchaEstado;

public class UpdateCanchaEstadoCommand : IRequest<bool>
{
    public long CanchaId { get; set; }
    public long UsuarioId { get; set; }
    public bool Estado { get; set; }
}

public class UpdateCanchaEstadoCommandHandler : IRequestHandler<UpdateCanchaEstadoCommand, bool>
{
    private readonly IApplicationDbContext _context;

    public UpdateCanchaEstadoCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(UpdateCanchaEstadoCommand request, CancellationToken cancellationToken)
    {
        var cancha = await _context.Canchas
            .Include(c => c.Complejo)
            .FirstOrDefaultAsync(c => c.Id == request.CanchaId, cancellationToken);

        if (cancha == null || cancha.Complejo == null || cancha.Complejo.DuenoId != request.UsuarioId)
        {
            return false;
        }

        cancha.Estado = request.Estado;
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
