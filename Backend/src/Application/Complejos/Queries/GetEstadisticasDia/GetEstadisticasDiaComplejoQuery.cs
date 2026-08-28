using MediatR;

namespace LaReservaBackend.Application.Complejos.Queries.GetEstadisticasDia;

public class GetEstadisticasDiaComplejoQuery : IRequest<EstadisticasDiaDto>
{
    public long ComplejoId { get; set; }
    public DateTime Fecha { get; set; }
    public long UsuarioId { get; set; }

    public GetEstadisticasDiaComplejoQuery(long complejoId, DateTime fecha, long usuarioId)
    {
        ComplejoId = complejoId;
        Fecha = fecha;
        UsuarioId = usuarioId;
    }
}
