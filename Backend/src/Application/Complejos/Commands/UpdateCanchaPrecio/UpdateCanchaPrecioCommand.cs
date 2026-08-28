using MediatR;
using LaReservaBackend.Application.Common.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace LaReservaBackend.Application.Complejos.Commands.UpdateCanchaPrecio;

public class UpdateCanchaPrecioCommand : IRequest<bool>
{
    public long CanchaId { get; set; }
    public long UsuarioId { get; set; }
    public decimal PrecioHora { get; set; }
}

public class UpdateCanchaPrecioCommandHandler : IRequestHandler<UpdateCanchaPrecioCommand, bool>
{
    private readonly IApplicationDbContext _context;

    public UpdateCanchaPrecioCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(UpdateCanchaPrecioCommand request, CancellationToken cancellationToken)
    {
        var cancha = await _context.Canchas
            .Include(c => c.Complejo)
            .FirstOrDefaultAsync(c => c.Id == request.CanchaId, cancellationToken);

        if (cancha == null || cancha.Complejo == null || cancha.Complejo.DuenoId != request.UsuarioId)
        {
            return false;
        }

        cancha.PrecioHora = request.PrecioHora;
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
