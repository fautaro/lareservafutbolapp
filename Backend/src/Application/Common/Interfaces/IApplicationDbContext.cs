using Microsoft.EntityFrameworkCore;

namespace LaReservaBackend.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    // Minimal contract — keep SaveChanges and a way to access DbSets in implementation.
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
