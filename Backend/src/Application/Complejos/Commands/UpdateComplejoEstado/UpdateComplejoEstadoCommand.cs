using MediatR;
using LaReservaBackend.Application.Common.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace LaReservaBackend.Application.Complejos.Commands.UpdateComplejoEstado;

public class UpdateComplejoEstadoCommand : IRequest<bool>
{
    public long ComplejoId { get; set; }
    public long UsuarioId { get; set; }
    public bool Estado { get; set; }
}

public class UpdateComplejoEstadoCommandHandler : IRequestHandler<UpdateComplejoEstadoCommand, bool>
{
    private readonly IApplicationDbContext _context;

    public UpdateComplejoEstadoCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(UpdateComplejoEstadoCommand request, CancellationToken cancellationToken)
    {
        var complejo = await _context.Complejos.FindAsync(new object[] { request.ComplejoId }, cancellationToken);
        if (complejo == null || complejo.DuenoId != request.UsuarioId)
        {
            return false;
        }

        complejo.Estado = request.Estado;
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
