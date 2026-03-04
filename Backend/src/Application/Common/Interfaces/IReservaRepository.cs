using LaReservaBackend.Application.Reservas.Queries;

namespace LaReservaBackend.Application.Common.Interfaces;

public interface IReservaRepository
{
    Task<GetHorariosDisponiblesResponse> GetHorariosDisponibles(long complejoId, CancellationToken cancellationToken = default);
    Task<GetUserReservationsResponse> GetUserReservations(long usuarioId, CancellationToken cancellationToken = default);
    Task<bool> CancelReservation(long reservaId, CancellationToken cancellationToken = default);
    Task<long> CreateReservation(LaReservaBackend.Domain.Entities.Reserva reserva, CancellationToken cancellationToken = default);
    Task<ReservaDetalleResponse?> GetNextReserva(long usuarioId, CancellationToken cancellationToken = default);
}
