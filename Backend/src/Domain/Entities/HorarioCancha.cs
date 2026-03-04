namespace LaReservaBackend.Domain.Entities;
public class HorarioCancha
{
    public long Id { get; set; }

    public long CanchaId { get; set; }
    public Cancha? Cancha { get; set; }

    public int DiaSemana { get; set; }  // 1=lunes ... 7=domingo
    public TimeSpan HoraInicio { get; set; }
    public TimeSpan HoraFin { get; set; }
    public bool Disponible { get; set; } = true;
}
