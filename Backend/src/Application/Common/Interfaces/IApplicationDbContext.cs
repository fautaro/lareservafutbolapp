using Microsoft.EntityFrameworkCore;
using LaReservaBackend.Domain.Entities;

namespace LaReservaBackend.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<TipoUsuario> TipoUsuarios { get; }
    DbSet<Usuario> Usuarios { get; }
    DbSet<Ciudad> Ciudades { get; }
    DbSet<Complejo> Complejos { get; }
    DbSet<TipoCancha> TipoCanchas { get; }
    DbSet<Cancha> Canchas { get; }
    DbSet<Reserva> Reservas { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
