using LaReservaBackend.Application.Common.Interfaces;

namespace LaReservaBackend.Application.Reservas.Queries;

public class GetNextReserva : IRequest<ReservaDetalleResponse?>
{
    public long UsuarioId { get; init; }

    public GetNextReserva(long usuarioId)
    {
        UsuarioId = usuarioId;
    }

    public class GetNextReservaHandler : IRequestHandler<GetNextReserva, ReservaDetalleResponse?>
    {
        private readonly IReservaRepository _reservaRepository;

        public GetNextReservaHandler(IReservaRepository reservaRepository)
        {
            _reservaRepository = reservaRepository;
        }

        public async Task<ReservaDetalleResponse?> Handle(GetNextReserva request, CancellationToken cancellationToken)
        {
            return await _reservaRepository.GetNextReserva(request.UsuarioId, cancellationToken);
        }
    }
}
