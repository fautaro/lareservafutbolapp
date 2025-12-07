namespace LaReservaBackend.Application.Common.Models.DTOs.Deportes;
public class DeporteDTO
{
    public long Id { get; init; }
    public string Nombre { get; init; } = string.Empty;
    public string Icon { get; init; } = string.Empty;
    public string BgClass { get; init; } = string.Empty;
    public string TextClass { get; init; } = string.Empty;
}
