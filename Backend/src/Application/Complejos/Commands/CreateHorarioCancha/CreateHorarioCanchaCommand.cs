using MediatR;
using LaReservaBackend.Application.Common.Interfaces;
using LaReservaBackend.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;
using System;

namespace LaReservaBackend.Application.Complejos.Commands.CreateHorarioCancha;

public class CreateHorarioCanchaCommand : IRequest<long>
{
    public long ComplejoId { get; set; }
    public long CanchaId { get; set; }
    public int DiaSemana { get; set; }
    public TimeSpan HoraInicio { get; set; }
    public TimeSpan HoraFin { get; set; }
    public bool Disponible { get; set; } = true;
}

public class CreateHorarioCanchaCommandHandler : IRequestHandler<CreateHorarioCanchaCommand, long>
{
    private readonly IApplicationDbContext _context;

    public CreateHorarioCanchaCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<long> Handle(CreateHorarioCanchaCommand request, CancellationToken cancellationToken)
    {
        var horario = new HorarioCancha
        {
            CanchaId = request.CanchaId,
            DiaSemana = request.DiaSemana,
            HoraInicio = request.HoraInicio,
            HoraFin = request.HoraFin,
            Disponible = request.Disponible
        };

        _context.HorariosCanchas.Add(horario);
        await _context.SaveChangesAsync(cancellationToken);

        return horario.Id;
    }
}
