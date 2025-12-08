using LaReservaBackend.Application.Common.Models;

namespace LaReservaBackend.Application.Common.Interfaces;
public interface IAuthService
{
    Task<AuthResultDto> AuthenticateAsync(string email, string password, string ipAddress);
    Task<AuthResultDto> RefreshTokenAsync(string token, string ipAddress);
    Task RevokeRefreshTokenAsync(string token, string ipAddress);
}
