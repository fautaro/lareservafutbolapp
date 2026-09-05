using LaReservaBackend.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LaReservaBackend.Application.Usuarios.Queries.GetUsuarios;

public record GetUsuariosQuery : IRequest<List<UsuarioDto>>;

public class GetUsuariosHandler : IRequestHandler<GetUsuariosQuery, List<UsuarioDto>>
{
    private readonly IApplicationDbContext _context;

    public GetUsuariosHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<UsuarioDto>> Handle(GetUsuariosQuery request, CancellationToken cancellationToken)
    {
        var usuarios = await _context.Usuarios
            .AsNoTracking()
            .Include(u => u.TipoUsuario)
            .OrderByDescending(u => u.Id)
            .Select(u => new UsuarioDto
            {
                Id = u.Id,
                Nombre = u.Nombre,
                Apellido = u.Apellido,
                Dni = u.Dni,
                Email = u.Email,
                Telefono = u.Telefono,
                Activo = u.Activo,
                TipoUsuarioId = u.TipoUsuarioId,
                TipoUsuarioNombre = u.TipoUsuario != null ? u.TipoUsuario.Nombre : "Jugador",
                FechaRegistro = u.FechaRegistro
            })
            .ToListAsync(cancellationToken);

        return usuarios;
    }
}
