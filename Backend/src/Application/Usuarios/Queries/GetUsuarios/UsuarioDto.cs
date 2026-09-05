namespace LaReservaBackend.Application.Usuarios.Queries.GetUsuarios;

public class UsuarioDto
{
    public long Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Apellido { get; set; }
    public string? Dni { get; set; }
    public string Email { get; set; } = string.Empty;
    public string? Telefono { get; set; }
    public bool Activo { get; set; } = true;
    public long TipoUsuarioId { get; set; }
    public string TipoUsuarioNombre { get; set; } = string.Empty;
    public DateTime FechaRegistro { get; set; }
}
