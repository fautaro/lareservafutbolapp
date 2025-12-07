using LaReservaBackend.Application.Common.Interfaces;
using MediatR;

namespace LaReservaBackend.Application.Complejos.Queries.GetComplejos;

public class GetComplejosRequest : IRequest<GetComplejosResponse>
{
    public int ComplejoId { get; init; }
    public int DeporteId { get; init; }

    public GetComplejosRequest(int complejoId, int deporteId)
    {
        ComplejoId = complejoId;
        DeporteId = deporteId;
    }
}

public class GetComplejosHandler : IRequestHandler<GetComplejosRequest, GetComplejosResponse>
{
    private readonly IComplejosRepository _complejosRepository;

    public GetComplejosHandler(IComplejosRepository complejosRepository)
    {
        _complejosRepository = complejosRepository;
    }

    public async Task<GetComplejosResponse> Handle(GetComplejosRequest request, CancellationToken cancellationToken)
    {
        var complejos = await _complejosRepository.GetComplejos(null, request.DeporteId != 0 ? request.DeporteId : null, request.ComplejoId != 0 ? request.ComplejoId : null);

        return new GetComplejosResponse
        {
            Deportes = new List<DeporteDto>(), // Completar si es necesario
            Complejos = complejos,
            Timestamp = DateTime.UtcNow,
        };
    }
}
