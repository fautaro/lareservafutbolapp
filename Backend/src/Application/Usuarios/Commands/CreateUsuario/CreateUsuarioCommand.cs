using LaReservaBackend.Application.Common.Interfaces;
using LaReservaBackend.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LaReservaBackend.Application.Usuarios.Commands.CreateUsuario;

public record CreateUsuarioCommand : IRequest<long>
{
    public string Nombre { get; init; } = string.Empty;
    public string? Apellido { get; init; }
    public string? Dni { get; init; }
    public string Email { get; init; } = string.Empty;
    public bool EsDueno { get; init; }
    public bool EsAdmin { get; init; }
}

public class CreateUsuarioCommandHandler : IRequestHandler<CreateUsuarioCommand, long>
{
    private readonly IApplicationDbContext _context;
    private readonly IPasswordHasher<Usuario> _passwordHasher;

    public CreateUsuarioCommandHandler(IApplicationDbContext context, IPasswordHasher<Usuario> passwordHasher)
    {
        _context = context;
        _passwordHasher = passwordHasher;
    }

    public async Task<long> Handle(CreateUsuarioCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Nombre))
        {
            throw new ArgumentException("El nombre es requerido.");
        }

        if (string.IsNullOrWhiteSpace(request.Email))
        {
            throw new ArgumentException("El email es requerido.");
        }

        var emailNormalized = request.Email.Trim().ToLower();

        var existingUser = await _context.Usuarios
            .AnyAsync(u => u.Email.ToLower() == emailNormalized, cancellationToken);

        if (existingUser)
        {
            throw new InvalidOperationException("Ya existe un usuario con ese correo electrónico.");
        }

        // Determinar rol
        long tipoUsuarioId = 1; // Por defecto Jugador / Cliente

        if (request.EsAdmin)
        {
            var adminTipo = await _context.TipoUsuarios
                .FirstOrDefaultAsync(t => t.Nombre.ToLower() == "admin", cancellationToken);
            tipoUsuarioId = adminTipo?.Id ?? 3;
        }
        else if (request.EsDueno)
        {
            // Busca 'Jugador y Dueño' o 'Dueño'
            var duenoTipo = await _context.TipoUsuarios
                .FirstOrDefaultAsync(t => t.Nombre.ToLower() == "jugador y dueño" || t.Nombre.ToLower() == "dueño" || t.Nombre.ToLower() == "dueno", cancellationToken);
            tipoUsuarioId = duenoTipo?.Id ?? 2;
        }
        else
        {
            var jugadorTipo = await _context.TipoUsuarios
                .FirstOrDefaultAsync(t => t.Nombre.ToLower() == "jugador" || t.Nombre.ToLower() == "cliente", cancellationToken);
            tipoUsuarioId = jugadorTipo?.Id ?? 1;
        }

        var nuevoUsuario = new Usuario
        {
            Nombre = request.Nombre.Trim(),
            Apellido = request.Apellido?.Trim(),
            Dni = request.Dni?.Trim(),
            Email = emailNormalized,
            Activo = true,
            Auth0Id = $"local|{Guid.NewGuid():N}",
            TipoUsuarioId = tipoUsuarioId,
            FechaRegistro = DateTime.UtcNow,
            DebeCambiarPassword = true
        };

        nuevoUsuario.PasswordHash = _passwordHasher.HashPassword(nuevoUsuario, "1234");

        _context.Usuarios.Add(nuevoUsuario);
        await _context.SaveChangesAsync(cancellationToken);

        return nuevoUsuario.Id;
    }
}
