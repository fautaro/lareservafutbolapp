using System.Collections.Generic;
using System.Threading.Tasks;
using LaReservaBackend.Application.Common.Interfaces;
using LaReservaBackend.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LaReservaBackend.Infrastructure.Repositories.Complejos;

public class ComplejosRepository
{
    private readonly IApplicationDbContext _context;

    public ComplejosRepository(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Complejo>> GetAllAsync()
    {
        return await _context.Complejos.ToListAsync();
    }

    public async Task<Complejo?> GetByIdAsync(int id)
    {
        return await _context.Complejos.FirstOrDefaultAsync(c => c.Id == id);
    }
}
