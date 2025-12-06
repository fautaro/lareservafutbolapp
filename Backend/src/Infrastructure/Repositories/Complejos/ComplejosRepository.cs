using LaReservaBackend.Application.Common.Interfaces;
using LaReservaBackend.Application.Common.Models.DTOs.Complejos;
using Microsoft.EntityFrameworkCore;

namespace LaReservaBackend.Infrastructure.Repositories.Complejos;

public class ComplejosRepository : IComplejosRepository
{
    private readonly IApplicationDbContext _context;

    public ComplejosRepository(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<ComplejoDTO>> GetComplejos(long? ciudadId = null, long? deporteId = null, long? complejoId = null)
    {
        var query = _context.Complejos
            .Where(c => c.Estado)
            .Include(c => c.Ciudad)
            .Include(c => c.Deporte)
            .AsQueryable();

        if (ciudadId.HasValue)
            query = query.Where(c => c.CiudadId == ciudadId.Value);
        if (deporteId.HasValue)
            query = query.Where(c => c.DeporteId == deporteId.Value);
        if (complejoId.HasValue)
            query = query.Where(c => c.Id == complejoId.Value);

        return await query
            .Select(c => new ComplejoDTO
            {
                Id = c.Id,
                Ciudad_Id = c.CiudadId,
                Nombre = c.Nombre,
                Precio = c.Precio != null ? "$" + c.Precio.Value.ToString("N0") : null,
                Imagen = c.Imagen,
                Categoria = c.Categoria != null ? c.Categoria : (c.Deporte != null ? c.Deporte.Nombre : null),
                DeportePillBg = c.DeportePillBg != null ? c.DeportePillBg : (c.Deporte != null ? c.Deporte.BgClass : null),
                DeportePillText = c.DeportePillText != null ? c.DeportePillText : (c.Deporte != null ? c.Deporte.TextClass : null)
            })
            .ToListAsync();
    }
}
