using LaReservaBackend.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LaReservaBackend.Application.Usuarios.Commands.BajaUsuario;

public record BajaUsuarioCommand(long Id) : IRequest<bool>;

public class BajaUsuarioCommandHandler : IRequestHandler<BajaUsuarioCommand, bool>
{
    private readonly IApplicationDbContext _context;

    public BajaUsuarioCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(BajaUsuarioCommand request, CancellationToken cancellationToken)
    {
        var usuario = await _context.Usuarios
            .FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken);

        if (usuario == null)
        {
            return false;
        }

        usuario.Activo = false;
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
