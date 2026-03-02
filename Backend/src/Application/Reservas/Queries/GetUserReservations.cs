using LaReservaBackend.Application.Common.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace LaReservaBackend.Application.Reservas.Queries;

public class GetUserReservations : IRequest<GetUserReservationsResponse>
{
    public long UsuarioId { get; init; }

    public GetUserReservations(long usuarioId)
    {
        UsuarioId = usuarioId;
    }

    public class GetUserReservationsHandler : IRequestHandler<GetUserReservations, GetUserReservationsResponse>
    {
        private readonly IReservaRepository _reservaRepository;

        public GetUserReservationsHandler(IReservaRepository reservaRepository)
        {
            _reservaRepository = reservaRepository;
        }

        public async Task<GetUserReservationsResponse> Handle(GetUserReservations request, CancellationToken cancellationToken)
        {
            return await _reservaRepository.GetUserReservations(request.UsuarioId, cancellationToken);
        }
    }
}
