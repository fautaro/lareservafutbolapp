using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaReservaBackend.Application.Common.Interfaces;
using LaReservaBackend.Application.Reservas.Queries;
using Microsoft.EntityFrameworkCore;

namespace LaReservaBackend.Infrastructure.Repositories.Reserva;
public class ReservaRepository : IReservaRepository
{
    private readonly IApplicationDbContext _context;

    public ReservaRepository(IApplicationDbContext context)
    {
      _context = context;
    }

    public async Task<GetHorariosDisponiblesResponse> GetHorariosDisponibles(long canchaId, CancellationToken cancellationToken = default)
    {
      // Obtener la cancha con su complejo para saber el MaxDiasDisponiblesReserva
        var cancha = await _context.Canchas
  .Include(c => c.Complejo)
            .FirstOrDefaultAsync(c => c.Id == canchaId, cancellationToken);

        if (cancha == null || cancha.Complejo == null)
        {
      return new GetHorariosDisponiblesResponse();
    }

    var maxDias = cancha.Complejo.MaxDiasDisponiblesReserva ?? 7; // Default 7 días si no está definido
        var fechaInicio = DateTime.SpecifyKind(DateTime.Today, DateTimeKind.Utc);
        var fechaFin = fechaInicio.AddDays(maxDias);

        // Obtener todos los horarios de la cancha
    var horariosCancha = await _context.HorariosCanchas
            .Where(h => h.CanchaId == canchaId && h.Disponible)
    .ToListAsync(cancellationToken);

   // Obtener todas las reservas confirmadas de la cancha en el rango de fechas
      var reservasConfirmadas = await _context.Reservas
       .Where(r => r.CanchaId == canchaId 
&& r.Confirmada 
       && r.Fecha >= fechaInicio 
       && r.Fecha < fechaFin)
  .Select(r => new { r.Fecha, r.FechaFin })
       .ToListAsync(cancellationToken);

var horariosPorDia = new List<HorarioPorDia>();

      // Iterar por cada día del rango
        for (int i = 0; i < maxDias; i++)
        {
var fechaActual = fechaInicio.AddDays(i);
   var diaSemana = ObtenerDiaSemana(fechaActual.DayOfWeek);

    // Obtener los horarios configurados para este día de la semana
        var horariosDelDia = horariosCancha
   .Where(h => h.DiaSemana == diaSemana)
    .ToList();

            var horariosDisponibles = new List<HorarioDisponible>();

        foreach (var horario in horariosDelDia)
          {
  // Crear la fecha y hora inicio y fin para esta reserva potencial
     var fechaHoraInicio = fechaActual.Date + horario.HoraInicio;
         var fechaHoraFin = fechaActual.Date + horario.HoraFin;

     // Verificar si hay conflicto con alguna reserva existente
    var hayConflicto = reservasConfirmadas.Any(r =>
               (fechaHoraInicio >= r.Fecha && fechaHoraInicio < r.FechaFin) ||
   (fechaHoraFin > r.Fecha && fechaHoraFin <= r.FechaFin) ||
               (fechaHoraInicio <= r.Fecha && fechaHoraFin >= r.FechaFin)
         );

      if (!hayConflicto)
      {
      horariosDisponibles.Add(new HorarioDisponible
              {
       HorarioCanchaId = horario.Id,
            HoraInicio = horario.HoraInicio,
               HoraFin = horario.HoraFin
        });
           }
            }

         if (horariosDisponibles.Any())
    {
                horariosPorDia.Add(new HorarioPorDia
                {
       Fecha = fechaActual,
     DiaSemana = diaSemana,
 Horarios = horariosDisponibles
    });
            }
        }

    return new GetHorariosDisponiblesResponse
{
            HorariosPorDia = horariosPorDia
  };
    }

    private int ObtenerDiaSemana(DayOfWeek dayOfWeek)
    {
        // Convertir DayOfWeek de .NET (Sunday=0) a formato base de datos (Monday=1)
return dayOfWeek switch
     {
        DayOfWeek.Monday => 1,
      DayOfWeek.Tuesday => 2,
     DayOfWeek.Wednesday => 3,
            DayOfWeek.Thursday => 4,
            DayOfWeek.Friday => 5,
          DayOfWeek.Saturday => 6,
          DayOfWeek.Sunday => 7,
      _ => 1
        };
    }
}
