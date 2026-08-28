using LaReservaBackend.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LaReservaBackend.Application.Complejos.Queries.GetComplejoConfig;

public class GetComplejoConfigQuery : IRequest<ComplejoConfigDto?>
{
    public long ComplejoId { get; set; }
    public long? UsuarioId { get; set; }
}

public class ComplejoConfigDto
{
    public long Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Direccion { get; set; }
    public long CiudadId { get; set; }
    public string? CiudadNombre { get; set; }
    public string? Categoria { get; set; }
    public string? Imagen { get; set; }
    public bool Estado { get; set; }
    public List<CanchaConfigDto> Canchas { get; set; } = new();
}

public class CanchaConfigDto
{
    public long Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public decimal? PrecioHora { get; set; }
    public string? TipoCancha { get; set; }
    public bool Estado { get; set; }
    public string? Descripcion { get; set; }
}

public class GetComplejoConfigHandler : IRequestHandler<GetComplejoConfigQuery, ComplejoConfigDto?>
{
    private readonly IApplicationDbContext _context;

    public GetComplejoConfigHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ComplejoConfigDto?> Handle(GetComplejoConfigQuery request, CancellationToken cancellationToken)
    {
        var complejo = await _context.Complejos
            .Include(c => c.Ciudad)
            .Include(c => c.Canchas)
                .ThenInclude(ca => ca.TipoCancha)
            .FirstOrDefaultAsync(c => c.Id == request.ComplejoId, cancellationToken);

        if (complejo == null) return null;

        if (request.UsuarioId.HasValue && complejo.DuenoId != request.UsuarioId.Value)
        {
            return null;
        }

        return new ComplejoConfigDto
        {
            Id = complejo.Id,
            Nombre = complejo.Nombre,
            Direccion = complejo.Direccion,
            CiudadId = complejo.CiudadId,
            CiudadNombre = complejo.Ciudad?.Nombre,
            Categoria = complejo.Categoria,
            Imagen = complejo.Imagen,
            Estado = complejo.Estado,
            Canchas = complejo.Canchas.Select(ca => new CanchaConfigDto
            {
                Id = ca.Id,
                Nombre = ca.Nombre,
                PrecioHora = ca.PrecioHora,
                TipoCancha = ca.TipoCancha?.Nombre,
                Estado = ca.Estado,
                Descripcion = ca.Descripcion
            }).ToList()
        };
    }
}
