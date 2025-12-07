using LaReservaBackend.Application.Common.Interfaces;
using LaReservaBackend.Application.Common.Models.DTOs.Deportes;
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
    private readonly IDeportesRepository _deportesRepository;

    public GetComplejosHandler(IComplejosRepository complejosRepository, IDeportesRepository deportesRepository)
    {
        _complejosRepository = complejosRepository;
        _deportesRepository = deportesRepository;
    }

    public async Task<GetComplejosResponse> Handle(GetComplejosRequest request, CancellationToken cancellationToken)
    {
        var complejos = await _complejosRepository.GetComplejos(null, request.DeporteId != 0 ? request.DeporteId : null, request.ComplejoId != 0 ? request.ComplejoId : null);
        var deportes = await _deportesRepository.GetDeportes();

        return new GetComplejosResponse
        {
            Deportes = deportes,
            Complejos = complejos,
            Timestamp = DateTime.UtcNow,
        };
    }
}
