using LaReservaBackend.Application.Common.Interfaces;
using LaReservaBackend.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LaReservaBackend.Application.Usuarios.Commands.CambiarPassword;

public record CambiarPasswordCommand : IRequest<bool>
{
    public long UsuarioId { get; init; }
    public string? PasswordActual { get; init; }
    public string NuevaPassword { get; init; } = string.Empty;
}

public class CambiarPasswordCommandHandler : IRequestHandler<CambiarPasswordCommand, bool>
{
    private readonly IApplicationDbContext _context;
    private readonly IPasswordHasher<Usuario> _passwordHasher;

    public CambiarPasswordCommandHandler(IApplicationDbContext context, IPasswordHasher<Usuario> passwordHasher)
    {
        _context = context;
        _passwordHasher = passwordHasher;
    }

    public async Task<bool> Handle(CambiarPasswordCommand request, CancellationToken cancellationToken)
    {
        if (request.UsuarioId <= 0)
        {
            throw new ArgumentException("El ID de usuario es inválido.");
        }

        if (string.IsNullOrWhiteSpace(request.NuevaPassword))
        {
            throw new ArgumentException("La nueva contraseña no puede estar vacía.");
        }

        if (request.NuevaPassword.Trim().Length < 4)
        {
            throw new ArgumentException("La nueva contraseña debe tener al menos 4 caracteres.");
        }

        var usuario = await _context.Usuarios
            .FirstOrDefaultAsync(u => u.Id == request.UsuarioId, cancellationToken);

        if (usuario == null)
        {
            throw new ArgumentException("Usuario no encontrado.");
        }

        // Si el usuario no tiene la bandera de cambio obligatorio y ya tenía contraseña, se puede verificar la actual si fue provista
        if (!usuario.DebeCambiarPassword && !string.IsNullOrEmpty(usuario.PasswordHash) && !string.IsNullOrEmpty(request.PasswordActual))
        {
            var verifyOld = _passwordHasher.VerifyHashedPassword(usuario, usuario.PasswordHash, request.PasswordActual);
            if (verifyOld == PasswordVerificationResult.Failed)
            {
                throw new ArgumentException("La contraseña actual es incorrecta.");
            }
        }

        usuario.PasswordHash = _passwordHasher.HashPassword(usuario, request.NuevaPassword.Trim());
        usuario.DebeCambiarPassword = false;

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
