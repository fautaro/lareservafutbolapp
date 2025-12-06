using LaReservaBackend.Domain.Entities;

namespace LaReservaBackend.Application.Common.Interfaces;
public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByIdAsync(Guid id);
    Task AddAsync(User user);
    Task UpdateAsync(User user);
    Task<User?> GetByRefreshTokenAsync(string refreshToken);

    Task SaveChangesAsync();
}
