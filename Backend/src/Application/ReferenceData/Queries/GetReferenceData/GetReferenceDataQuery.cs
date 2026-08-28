using LaReservaBackend.Application.Common.Interfaces;
using LaReservaBackend.Application.Common.Models.DTOs.Deportes;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LaReservaBackend.Application.ReferenceData.Queries.GetReferenceData;

public class GetReferenceDataQuery : IRequest<ReferenceDataResponse>
{
}

public class ReferenceDataResponse
{
    public List<ReferenceItemDto> Ciudades { get; set; } = new();
    public List<DeporteDTO> Deportes { get; set; } = new();
    public List<ReferenceItemDto> TiposCancha { get; set; } = new();
}

public class ReferenceItemDto
{
    public long Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
}

public class GetReferenceDataHandler : IRequestHandler<GetReferenceDataQuery, ReferenceDataResponse>
{
    private readonly IApplicationDbContext _context;

    public GetReferenceDataHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ReferenceDataResponse> Handle(GetReferenceDataQuery request, CancellationToken cancellationToken)
    {
        var ciudades = await _context.Ciudades
            .Select(c => new ReferenceItemDto { Id = c.Id, Nombre = c.Nombre })
            .ToListAsync(cancellationToken);

        var tiposCancha = await _context.TipoCanchas
            .Select(t => new ReferenceItemDto { Id = t.Id, Nombre = t.Nombre })
            .ToListAsync(cancellationToken);

        var deportes = await _context.Deportes
            .Select(d => new DeporteDTO
            {
                Id = d.Id,
                Nombre = d.Nombre,
                Icon = d.Icon ?? string.Empty,
                BgClass = d.BgClass ?? string.Empty,
                TextClass = d.TextClass ?? string.Empty
            })
            .ToListAsync(cancellationToken);

        return new ReferenceDataResponse
        {
            Ciudades = ciudades,
            Deportes = deportes,
            TiposCancha = tiposCancha
        };
    }
}
