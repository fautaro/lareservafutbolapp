using LaReservaBackend.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LaReservaBackend.Application.Usuarios.Commands.ReactivarUsuario;

public record ReactivarUsuarioCommand(long Id) : IRequest<bool>;

public class ReactivarUsuarioCommandHandler : IRequestHandler<ReactivarUsuarioCommand, bool>
{
    private readonly IApplicationDbContext _context;

    public ReactivarUsuarioCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(ReactivarUsuarioCommand request, CancellationToken cancellationToken)
    {
        var usuario = await _context.Usuarios
            .FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken);

        if (usuario == null)
        {
            return false;
        }

        usuario.Activo = true;
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
