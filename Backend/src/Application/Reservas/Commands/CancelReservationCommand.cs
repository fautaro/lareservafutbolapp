using LaReservaBackend.Application.Common.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace LaReservaBackend.Application.Reservas.Commands;

public class CancelReservationCommand : IRequest<bool>
{
    public long ReservaId { get; init; }

    public CancelReservationCommand(long reservaId)
    {
        ReservaId = reservaId;
    }

    public class CancelReservationHandler : IRequestHandler<CancelReservationCommand, bool>
    {
        private readonly IReservaRepository _reservaRepository;

        public CancelReservationHandler(IReservaRepository reservaRepository)
        {
            _reservaRepository = reservaRepository;
        }

        public async Task<bool> Handle(CancelReservationCommand request, CancellationToken cancellationToken)
        {
            return await _reservaRepository.CancelReservation(request.ReservaId, cancellationToken);
        }
    }
}
