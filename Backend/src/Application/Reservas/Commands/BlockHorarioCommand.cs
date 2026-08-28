using LaReservaBackend.Application.Common.Interfaces;
using LaReservaBackend.Domain.Entities;
using LaReservaBackend.Domain.Enums;
using MediatR;

namespace LaReservaBackend.Application.Reservas.Commands;

public class BlockHorarioCommand : IRequest<long>
{
    public long UsuarioId { get; set; }
    public long ComplejoId { get; set; }
    public long CanchaId { get; set; }
    public DateTime Fecha { get; set; }
    public TimeSpan HoraInicio { get; set; }
    public TimeSpan HoraFin { get; set; }
}

public class BlockHorarioHandler : IRequestHandler<BlockHorarioCommand, long>
{
    private readonly IReservaRepository _reservaRepository;

    public BlockHorarioHandler(IReservaRepository reservaRepository)
    {
        _reservaRepository = reservaRepository;
    }

    public async Task<long> Handle(BlockHorarioCommand request, CancellationToken cancellationToken)
    {
        var fechaInicio = DateTime.SpecifyKind(request.Fecha.Date + request.HoraInicio, DateTimeKind.Unspecified);
        var fechaFin = DateTime.SpecifyKind(request.Fecha.Date + request.HoraFin, DateTimeKind.Unspecified);

        var reserva = new Reserva
        {
            UsuarioId = request.UsuarioId,
            ComplejoId = request.ComplejoId,
            CanchaId = request.CanchaId,
            Fecha = fechaInicio,
            FechaFin = fechaFin,
            MontoTotal = 0,
            MedioPagoId = null,
            Estado = EstadoReserva.Bloqueado,
            FechaReserva = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Unspecified),
            Confirmada = false,
            EstadoPago = EstadoPago.pendiente
        };

        return await _reservaRepository.CreateReservation(reserva, cancellationToken);
    }
}
