using LaReservaBackend.Application.Common.Interfaces;

namespace LaReservaBackend.Application.Complejos.Queries.GetComplejos;

public class GetComplejos : IRequest<GetComplejosResponse>
{
    public int ComplejoId { get; init; }
    public int DeporteId { get; init; }
    public long? DuenoId { get; init; }

    public GetComplejos(int complejoId, int deporteId, long? duenoId = null)
    {
        ComplejoId = complejoId;
        DeporteId = deporteId;
        DuenoId = duenoId;
    }
}

public class GetComplejosHandler : IRequestHandler<GetComplejos, GetComplejosResponse>
{
    private readonly IComplejosRepository _complejosRepository;
    private readonly IDeportesRepository _deportesRepository;

    public GetComplejosHandler(IComplejosRepository complejosRepository, IDeportesRepository deportesRepository)
    {
        _complejosRepository = complejosRepository;
        _deportesRepository = deportesRepository;
    }

    public async Task<GetComplejosResponse> Handle(GetComplejos request, CancellationToken cancellationToken)
    {
        var complejos = await _complejosRepository.GetComplejos(
            cancellationToken: cancellationToken,
            deporteId: request.DeporteId != 0 ? request.DeporteId : null,
            complejoId: request.ComplejoId != 0 ? request.ComplejoId : null,
            duenoId: request.DuenoId
        );
        var deportes = await _deportesRepository.GetDeportes(cancellationToken);

        return new GetComplejosResponse
        {
            Deportes = deportes,
            Complejos = complejos,
            Timestamp = DateTime.UtcNow,
        };
    }
}
