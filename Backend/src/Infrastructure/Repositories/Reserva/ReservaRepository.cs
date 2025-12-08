using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaReservaBackend.Application.Common.Interfaces;

namespace LaReservaBackend.Infrastructure.Repositories.Reserva;
public class ReservaRepository : IReservaRepository
{
    private readonly IApplicationDbContext _context;

    public ReservaRepository(IApplicationDbContext context)
    {
        _context = context;
    }
}
