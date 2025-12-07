using LaReservaBackend.Application.Common.Models.DTOs.Complejos;

namespace LaReservaBackend.Application.Complejos.Queries.GetComplejos;

public class GetComplejosResponse
{
    public List<DeporteDto> Deportes { get; init; } = new();
    public List<ComplejoDTO> Complejos { get; init; } = new();
    public DateTime Timestamp { get; init; }
}

public class DeporteDto
{
    public int Id { get; init; }
    public string Nombre { get; init; } = string.Empty;
    public string Icon { get; init; } = string.Empty;
    public string BgClass { get; init; } = string.Empty;
    public string TextClass { get; init; } = string.Empty;
}
