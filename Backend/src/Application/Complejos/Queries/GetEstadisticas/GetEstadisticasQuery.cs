using MediatR;
using LaReservaBackend.Application.Complejos.Queries.GetEstadisticasDia;

namespace LaReservaBackend.Application.Complejos.Queries.GetEstadisticas;

public class GetEstadisticasQuery : IRequest<EstadisticasDiaDto> // Reusing the same DTO as it's structurally identical
{
    public long? ComplejoId { get; set; }
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
    public long UsuarioId { get; set; }

    public GetEstadisticasQuery(long? complejoId, DateTime fechaInicio, DateTime fechaFin, long usuarioId)
    {
        ComplejoId = complejoId;
        FechaInicio = fechaInicio;
        FechaFin = fechaFin;
        UsuarioId = usuarioId;
    }
}
