using System;

namespace LaReservaBackend.Application.Usuarios.Queries;

public class GetUserProfileResponse
{
    public long Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Telefono { get; set; }
    public string TipoUsuario { get; set; } = string.Empty;
    public DateTime FechaRegistro { get; set; }
}
