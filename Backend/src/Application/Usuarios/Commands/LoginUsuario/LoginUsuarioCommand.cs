using LaReservaBackend.Application.Common.Interfaces;
using LaReservaBackend.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LaReservaBackend.Application.Usuarios.Commands.LoginUsuario;

public record LoginUsuarioCommand : IRequest<LoginUsuarioResponse>
{
    public string Email { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
}

public class LoginUsuarioResponse
{
    public long Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Apellido { get; set; }
    public string Email { get; set; } = string.Empty;
    public string? Telefono { get; set; }
    public string Rol { get; set; } = "user";
    public string TipoUsuario { get; set; } = string.Empty;
    public bool DebeCambiarPassword { get; set; }
}

public class LoginUsuarioCommandHandler : IRequestHandler<LoginUsuarioCommand, LoginUsuarioResponse>
{
    private readonly IApplicationDbContext _context;
    private readonly IPasswordHasher<Usuario> _passwordHasher;

    public LoginUsuarioCommandHandler(IApplicationDbContext context, IPasswordHasher<Usuario> passwordHasher)
    {
        _context = context;
        _passwordHasher = passwordHasher;
    }

    public async Task<LoginUsuarioResponse> Handle(LoginUsuarioCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
        {
            throw new ArgumentException("El email y la contraseña son requeridos.");
        }

        var emailNormalized = request.Email.Trim().ToLower();

        var usuario = await _context.Usuarios
            .Include(u => u.TipoUsuario)
            .FirstOrDefaultAsync(u => u.Email.ToLower() == emailNormalized, cancellationToken);

        if (usuario == null)
        {
            throw new UnauthorizedAccessException("Credenciales inválidas o usuario no encontrado.");
        }

        if (!usuario.Activo)
        {
            throw new UnauthorizedAccessException("El usuario se encuentra inactivo. Contacte al administrador.");
        }

        if (string.IsNullOrEmpty(usuario.PasswordHash))
        {
            throw new UnauthorizedAccessException("Este usuario no tiene contraseña registrada. Por favor ingrese con su método habitual.");
        }

        var verificationResult = _passwordHasher.VerifyHashedPassword(usuario, usuario.PasswordHash, request.Password);
        if (verificationResult == PasswordVerificationResult.Failed)
        {
            throw new UnauthorizedAccessException("Credenciales inválidas.");
        }

        // Determinar rol frontend ('admin', 'owner', 'user')
        var tipoNombre = (usuario.TipoUsuario?.Nombre ?? "").ToLower();
        string rol = "user";
        if (tipoNombre.Contains("admin"))
        {
            rol = "admin";
        }
        else if (tipoNombre.Contains("dueño") || tipoNombre.Contains("dueno"))
        {
            rol = "owner";
        }

        return new LoginUsuarioResponse
        {
            Id = usuario.Id,
            Nombre = usuario.Nombre,
            Apellido = usuario.Apellido,
            Email = usuario.Email,
            Telefono = usuario.Telefono,
            Rol = rol,
            TipoUsuario = usuario.TipoUsuario?.Nombre ?? "Cliente",
            DebeCambiarPassword = usuario.DebeCambiarPassword
        };
    }
}
