using LaReservaBackend.Application.Common.Interfaces;
using LaReservaBackend.Domain.Entities;

namespace LaReservaBackend.Infrastructure.Repositories;
public class UserRepository : IUserRepository
{
    //private readonly AppDbContext _ctx;
    //public UserRepository(AppDbContext ctx) { _ctx = ctx; }

    public async Task AddAsync(User user)
    {
        //_ctx.Users.Add(user);
        await Task.CompletedTask;
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        //return await _ctx.Users.Include(u => u.RefreshTokens).FirstOrDefaultAsync(u => u.Email == email);
        await Task.CompletedTask;
        return new User();
    }

    public async Task<User?> GetByIdAsync(Guid id)
    {
        //return await _ctx.Users.Include(u => u.RefreshTokens).FirstOrDefaultAsync(u => u.Id == id);
        await Task.CompletedTask;
        return new User();
    }

    public async Task<User?> GetByRefreshTokenAsync(string refreshToken)
    {
        //return await _ctx.Users.Include(u => u.RefreshTokens)
        //       .FirstOrDefaultAsync(u => u.RefreshTokens.Any(rt => rt.Token == refreshToken));
        await Task.CompletedTask;
        return new User();
    }

    public Task UpdateAsync(User user)
    {
        //_ctx.Users.Update(user);
        return Task.CompletedTask;
    }

    public Task SaveChangesAsync()
    {
        //return _ctx.SaveChangesAsync();
        return Task.CompletedTask;
    }
}
