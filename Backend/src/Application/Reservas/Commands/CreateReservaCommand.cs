using LaReservaBackend.Application.Common.Interfaces;
using LaReservaBackend.Domain.Entities;
using LaReservaBackend.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace LaReservaBackend.Application.Reservas.Commands;

public class CreateReservaCommand : IRequest<long>
{
    public long UsuarioId { get; set; }
    public long ComplejoId { get; set; }
    public long CanchaId { get; set; }
    public DateTime Fecha { get; set; }
    public TimeSpan HoraInicio { get; set; }
    public TimeSpan HoraFin { get; set; }
    // MontoTotal ya no se acepta del frontend; se calcula en el backend a partir de Cancha.PrecioHora.
    public int MedioPagoId { get; set; }
}

public class CreateReservaHandler : IRequestHandler<CreateReservaCommand, long>
{
    private readonly IReservaRepository _reservaRepository;
    private readonly IApplicationDbContext _context;

    public CreateReservaHandler(IReservaRepository reservaRepository, IApplicationDbContext context)
    {
        _reservaRepository = reservaRepository;
        _context = context;
    }

    public async Task<long> Handle(CreateReservaCommand request, CancellationToken cancellationToken)
    {
        // Obtener el precio por hora de la cancha desde la base de datos
        var cancha = await _context.Canchas
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == request.CanchaId, cancellationToken);

        if (cancha == null)
        {
            throw new KeyNotFoundException($"Cancha {request.CanchaId} no encontrada.");
        }

        // Calcular el monto total basado en el precio por hora y la duración
        var duracionHoras = (decimal)(request.HoraFin - request.HoraInicio).TotalHours;
        var montoTotal = (cancha.PrecioHora ?? 0) * duracionHoras;

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
            MontoTotal = montoTotal,
            MedioPagoId = request.MedioPagoId,
            Estado = EstadoReserva.Pendiente,
            FechaReserva = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Unspecified),
            Confirmada = false,
            EstadoPago = EstadoPago.pendiente
        };

        return await _reservaRepository.CreateReservation(reserva, cancellationToken);
    }
}
