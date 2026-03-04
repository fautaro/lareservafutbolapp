using LaReservaBackend.Application.Common.Interfaces;

namespace LaReservaBackend.Application.Usuarios.Queries;

public record GetUserProfile(long UsuarioId) : IRequest<GetUserProfileResponse?>;

public class GetUserProfileHandler : IRequestHandler<GetUserProfile, GetUserProfileResponse?>
{
    private readonly IUsuarioRepository _usuarioRepository;

    public GetUserProfileHandler(IUsuarioRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
    }

    public async Task<GetUserProfileResponse?> Handle(GetUserProfile request, CancellationToken cancellationToken)
    {
        return await _usuarioRepository.GetUserProfileById(request.UsuarioId, cancellationToken);
    }
}
