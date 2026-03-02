using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LaReservaBackend.Application.Common.Interfaces;
using LaReservaBackend.Application.Usuarios.Queries;
using Microsoft.EntityFrameworkCore;

namespace LaReservaBackend.Infrastructure.Repositories.Usuarios;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly IApplicationDbContext _context;

    public UsuarioRepository(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<GetUserProfileResponse?> GetUserProfileById(long id, CancellationToken cancellationToken = default)
    {
        var usuario = await _context.Usuarios
            .Include(u => u.TipoUsuario)
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);

        if (usuario == null) return null;

        return new GetUserProfileResponse
        {
            Id = usuario.Id,
            Nombre = usuario.Nombre,
            Email = usuario.Email,
            Telefono = usuario.Telefono,
            TipoUsuario = usuario.TipoUsuario?.Nombre ?? "Cliente",
            FechaRegistro = usuario.FechaRegistro
        };
    }
}
