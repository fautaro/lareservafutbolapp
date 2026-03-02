using LaReservaBackend.Application.Common.Interfaces;
using LaReservaBackend.Domain.Entities;
using LaReservaBackend.Domain.Enums;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace LaReservaBackend.Application.Reservas.Commands;

public class CreateReservaCommand : IRequest<long>
{
    public long UsuarioId { get; set; }
    public long ComplejoId { get; set; }
    public long CanchaId { get; set; }
    public DateTime Fecha { get; set; }
    public TimeSpan HoraInicio { get; set; }
    public TimeSpan HoraFin { get; set; }
    public decimal MontoTotal { get; set; }
}

public class CreateReservaHandler : IRequestHandler<CreateReservaCommand, long>
{
    private readonly IReservaRepository _reservaRepository;

    public CreateReservaHandler(IReservaRepository reservaRepository)
    {
        _reservaRepository = reservaRepository;
    }

    public async Task<long> Handle(CreateReservaCommand request, CancellationToken cancellationToken)
    {
        // Usar Kind = Unspecified para que se guarde la hora "tal cual" en columnas sin zona horaria (timestamp without time zone)
        // Esto evita que Npgsql o Postgres resten horas para convertir a UTC.
        var fechaInicio = DateTime.SpecifyKind(request.Fecha.Date + request.HoraInicio, DateTimeKind.Unspecified);
        var fechaFin = DateTime.SpecifyKind(request.Fecha.Date + request.HoraFin, DateTimeKind.Unspecified);

        var reserva = new Reserva
        {
            UsuarioId = request.UsuarioId,
            ComplejoId = request.ComplejoId,
            CanchaId = request.CanchaId,
            Fecha = fechaInicio,
            FechaFin = fechaFin,
            MontoTotal = request.MontoTotal,
            Estado = EstadoReserva.Confirmado,
            FechaReserva = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Unspecified),
            Confirmada = true,
            EstadoPago = EstadoPago.pendiente
        };

        return await _reservaRepository.CreateReservation(reserva, cancellationToken);
    }
}
