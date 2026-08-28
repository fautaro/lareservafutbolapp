using MediatR;

namespace LaReservaBackend.Application.Complejos.Queries.GetAgendaComplejo;

public class GetAgendaComplejoQuery : IRequest<OwnerAgendaResponse>
{
    public long ComplejoId { get; set; }
    public DateTime Fecha { get; set; }
    public long UsuarioId { get; set; }

    public GetAgendaComplejoQuery(long complejoId, DateTime fecha, long usuarioId)
    {
        ComplejoId = complejoId;
        Fecha = fecha;
        UsuarioId = usuarioId;
    }
}
