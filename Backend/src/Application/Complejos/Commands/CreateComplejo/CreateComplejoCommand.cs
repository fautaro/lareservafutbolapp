using LaReservaBackend.Application.Common.Interfaces;
using LaReservaBackend.Domain.Entities;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace LaReservaBackend.Application.Complejos.Commands.CreateComplejo;

public class CreateComplejoCommand : IRequest<long>
{
    public long DuenoId { get; set; }
    [Required]
    public string Nombre { get; set; } = null!;
    public string? Direccion { get; set; }
    public long CiudadId { get; set; }
    public long? DeporteId { get; set; }
    public decimal? Precio { get; set; }
    public string? Imagen { get; set; }
    public string? Categoria { get; set; }
}

public class CreateComplejoCommandHandler : IRequestHandler<CreateComplejoCommand, long>
{
    private readonly IApplicationDbContext _context;

    public CreateComplejoCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<long> Handle(CreateComplejoCommand request, CancellationToken cancellationToken)
    {
        var entity = new Complejo
        {
            DuenoId = request.DuenoId,
            Nombre = request.Nombre,
            Direccion = request.Direccion,
            CiudadId = request.CiudadId,
            DeporteId = request.DeporteId,
            Precio = request.Precio,
            Imagen = request.Imagen,
            Categoria = request.Categoria,
            Estado = true,
            FechaCreacion = DateTime.UtcNow
        };

        if (request.DeporteId.HasValue && request.DeporteId.Value > 0)
        {
            var deporte = await _context.Deportes.FindAsync(new object[] { request.DeporteId.Value }, cancellationToken);
            if (deporte != null)
            {
                entity.DeportePillBg = deporte.BgClass;
                entity.DeportePillText = deporte.TextClass;
                if (string.IsNullOrEmpty(entity.Categoria))
                {
                    entity.Categoria = deporte.Nombre;
                }
            }
        }

        _context.Complejos.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
