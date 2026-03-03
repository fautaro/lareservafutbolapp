using LaReservaBackend.Domain.Entities;

namespace LaReservaBackend.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<TipoUsuario> TipoUsuarios { get; }
    DbSet<Usuario> Usuarios { get; }
    DbSet<Ciudad> Ciudades { get; }
    DbSet<Complejo> Complejos { get; }
    DbSet<Deporte> Deportes { get; }
    DbSet<TipoCancha> TipoCanchas { get; }
    DbSet<Cancha> Canchas { get; }
    DbSet<Reserva> Reservas { get; }
    DbSet<HorarioCancha> HorariosCanchas { get; }
    DbSet<MedioPago> MedioPagos { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
