using LaReservaBackend.Application.Common.Interfaces;
using LaReservaBackend.Application.Common.Models.DTOs.Complejos;
using LaReservaBackend.Application.Common.Models.DTOs.Deportes;
using Microsoft.EntityFrameworkCore;

namespace LaReservaBackend.Infrastructure.Repositories.Deportes;
public class DeportesRepository : IDeportesRepository
{
    private readonly IApplicationDbContext _context;

    public DeportesRepository(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<DeporteDTO>> GetDeportes()
    {
        return await _context.Deportes
            .Select(c => new DeporteDTO
            {
                Id = c.Id,
                Nombre = c.Nombre,
                Icon = c.Icon ?? string.Empty,
                BgClass = c.BgClass ?? string.Empty,
                TextClass = c.TextClass ?? string.Empty,
            })
            .ToListAsync();
    }
}
