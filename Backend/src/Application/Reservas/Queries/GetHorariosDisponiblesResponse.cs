using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaReservaBackend.Application.Reservas.Queries;

public class GetHorariosDisponiblesResponse
{
    public List<HorarioPorDia> HorariosPorDia { get; set; } = new();
}

public class HorarioPorDia
{
    public DateTime Fecha { get; set; }
    public int DiaSemana { get; set; }
    public List<HorarioDisponible> Horarios { get; set; } = new();
}

public class HorarioDisponible
{
    public long HorarioCanchaId { get; set; }
    public TimeSpan HoraInicio { get; set; }
    public TimeSpan HoraFin { get; set; }
    public string TipoCancha { get; set; } = string.Empty;
}
