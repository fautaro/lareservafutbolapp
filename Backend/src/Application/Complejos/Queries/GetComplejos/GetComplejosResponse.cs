using LaReservaBackend.Application.Common.Models.DTOs.Complejos;
using LaReservaBackend.Application.Common.Models.DTOs.Deportes;

namespace LaReservaBackend.Application.Complejos.Queries.GetComplejos;

public class GetComplejosResponse
{
    public List<DeporteDTO> Deportes { get; init; } = new();
    public List<ComplejoDTO> Complejos { get; init; } = new();
    public DateTime Timestamp { get; init; }
}
