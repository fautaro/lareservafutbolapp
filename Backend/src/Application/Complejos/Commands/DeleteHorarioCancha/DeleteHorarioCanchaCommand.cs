using MediatR;
using LaReservaBackend.Application.Common.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace LaReservaBackend.Application.Complejos.Commands.DeleteHorarioCancha;

public class DeleteHorarioCanchaCommand : IRequest<bool>
{
    public long ComplejoId { get; set; }
    public long CanchaId { get; set; }
    public long HorarioId { get; set; }
}

public class DeleteHorarioCanchaCommandHandler : IRequestHandler<DeleteHorarioCanchaCommand, bool>
{
    private readonly IApplicationDbContext _context;

    public DeleteHorarioCanchaCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(DeleteHorarioCanchaCommand request, CancellationToken cancellationToken)
    {
        var horario = await _context.HorariosCanchas.FindAsync(new object[] { request.HorarioId }, cancellationToken);
        
        if (horario == null || horario.CanchaId != request.CanchaId)
        {
            return false;
        }

        _context.HorariosCanchas.Remove(horario);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
