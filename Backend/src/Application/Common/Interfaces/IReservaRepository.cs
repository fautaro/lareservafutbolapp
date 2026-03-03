using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaReservaBackend.Application.Reservas.Queries;
using LaReservaBackend.Domain.Entities;

namespace LaReservaBackend.Application.Common.Interfaces;

public interface IReservaRepository
{
    Task<GetHorariosDisponiblesResponse> GetHorariosDisponibles(long complejoId, CancellationToken cancellationToken = default);
    Task<GetUserReservationsResponse> GetUserReservations(long usuarioId, CancellationToken cancellationToken = default);
    Task<bool> CancelReservation(long reservaId, CancellationToken cancellationToken = default);
    Task<long> CreateReservation(LaReservaBackend.Domain.Entities.Reserva reserva, CancellationToken cancellationToken = default);
    Task<ReservaDetalleResponse?> GetNextReserva(long usuarioId, CancellationToken cancellationToken = default);
}
