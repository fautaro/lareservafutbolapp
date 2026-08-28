using MediatR;

namespace LaReservaBackend.Application.Complejos.Queries.GetAgendaComplejo;

public class GetAgendaComplejoQuery : IRequest<OwnerAgendaResponse>
{
    public long ComplejoId { get; set; }
    public DateTime Fecha { get; set; }
    public long UsuarioId { get; set; }
    public bool SoloReservas { get; set; }

    public GetAgendaComplejoQuery(long complejoId, DateTime fecha, long usuarioId, bool soloReservas = false)
    {
        ComplejoId = complejoId;
        Fecha = fecha;
        UsuarioId = usuarioId;
        SoloReservas = soloReservas;
    }
}
