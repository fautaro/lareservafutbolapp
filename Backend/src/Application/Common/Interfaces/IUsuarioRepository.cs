using System.Threading;
using System.Threading.Tasks;
using LaReservaBackend.Application.Usuarios.Queries;

namespace LaReservaBackend.Application.Common.Interfaces;

public interface IUsuarioRepository
{
    Task<GetUserProfileResponse?> GetUserProfileById(long id, CancellationToken cancellationToken = default);
}
