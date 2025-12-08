using LaReservaBackend.Application.Common.Interfaces;
using LaReservaBackend.Domain.Entities;
using LaReservaBackend.Domain.Enums;
using LaReservaBackend.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LaReservaBackend.Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<TipoUsuario> TipoUsuarios => Set<TipoUsuario>();
    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Ciudad> Ciudades => Set<Ciudad>();
    public DbSet<Complejo> Complejos => Set<Complejo>();
    public DbSet<Deporte> Deportes => Set<Deporte>();
    public DbSet<TipoCancha> TipoCanchas => Set<TipoCancha>();
    public DbSet<Cancha> Canchas => Set<Cancha>();
    public DbSet<Reserva> Reservas => Set<Reserva>();
    public DbSet<HorarioCancha> HorariosCanchas => Set<HorarioCancha>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // TipoUsuario
        modelBuilder.Entity<TipoUsuario>(b =>
        {
            b.ToTable("tipousuario");
            b.Property(x => x.Id).HasColumnName("id");
            b.Property(x => x.Nombre).HasColumnName("nombre").HasMaxLength(20).IsRequired();
            b.HasIndex(x => x.Nombre).IsUnique();
        });

        // Usuario
        modelBuilder.Entity<Usuario>(b =>
        {
            b.ToTable("usuario");
            b.Property(x => x.Id).HasColumnName("id");
            b.Property(x => x.Auth0Id).HasColumnName("auth0id").HasMaxLength(128).IsRequired();
            b.Property(x => x.Nombre).HasColumnName("nombre").HasMaxLength(100).IsRequired();
            b.Property(x => x.Email).HasColumnName("email").HasMaxLength(100).IsRequired();
            b.Property(x => x.Telefono).HasColumnName("telefono").HasMaxLength(20);
            b.Property(x => x.TipoUsuarioId).HasColumnName("tipousuarioid");
            b.Property(x => x.FechaRegistro).HasColumnName("fecharegistro").HasDefaultValueSql("CURRENT_TIMESTAMP");
            b.HasIndex(x => x.Auth0Id).IsUnique();
            b.HasIndex(x => x.Email).IsUnique();
            b.HasOne(x => x.TipoUsuario)
             .WithMany(t => t.Usuarios)
             .HasForeignKey(x => x.TipoUsuarioId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        // Ciudad
        modelBuilder.Entity<Ciudad>(b =>
        {
            b.ToTable("ciudad");
            b.Property(x => x.Id).HasColumnName("id");
            b.Property(x => x.Nombre).HasColumnName("nombre").HasMaxLength(100).IsRequired();
            b.HasIndex(x => x.Nombre).IsUnique();
        });

        // Deporte
        modelBuilder.Entity<Deporte>(b =>
        {
            b.ToTable("deporte");
            b.Property(x => x.Id).HasColumnName("id");
            b.Property(x => x.Nombre).HasColumnName("nombre").HasMaxLength(100).IsRequired();
            b.Property(x => x.Icon).HasColumnName("icon").HasMaxLength(200);
            b.Property(x => x.BgClass).HasColumnName("bgclass").HasMaxLength(100);
            b.Property(x => x.TextClass).HasColumnName("textclass").HasMaxLength(100);
            b.HasIndex(x => x.Nombre).IsUnique();
        });

        // Complejo
        modelBuilder.Entity<Complejo>(b =>
        {
            b.ToTable("complejo");
            b.Property(x => x.Id).HasColumnName("id");
            b.Property(x => x.Nombre).HasColumnName("nombre").HasMaxLength(100).IsRequired();
            b.Property(x => x.Direccion).HasColumnName("direccion").HasMaxLength(200);
            b.Property(x => x.CiudadId).HasColumnName("ciudadid");
            b.Property(x => x.DuenoId).HasColumnName("duenoid");
            b.Property(x => x.DeporteId).HasColumnName("deporteid");
            b.Property(x => x.Precio).HasColumnName("precio").HasColumnType("decimal(10,2)");
            b.Property(x => x.Imagen).HasColumnName("imagen");
            b.Property(x => x.Categoria).HasColumnName("categoria").HasMaxLength(100);
            b.Property(x => x.DeportePillBg).HasColumnName("deportepillbg").HasMaxLength(100);
            b.Property(x => x.DeportePillText).HasColumnName("deportepilltext").HasMaxLength(100);
            b.Property(x => x.Estado).HasColumnName("estado").HasDefaultValue(true);
            b.Property(x => x.MaxDiasDisponiblesReserva).HasColumnName("maxdiasdisponiblesreserva");
            b.Property(x => x.FechaCreacion).HasColumnName("fechacreacion").HasDefaultValueSql("CURRENT_TIMESTAMP");
            b.HasOne(x => x.Ciudad)
             .WithMany(c => c.Complejos)
             .HasForeignKey(x => x.CiudadId)
             .OnDelete(DeleteBehavior.Restrict);

            b.HasOne(x => x.Dueno)
             .WithMany(u => u.ComplejosComoDueno)
             .HasForeignKey(x => x.DuenoId)
             .OnDelete(DeleteBehavior.Restrict);

            b.HasOne(x => x.Deporte)
             .WithMany(d => d.Complejos)
             .HasForeignKey(x => x.DeporteId)
             .OnDelete(DeleteBehavior.SetNull);
        });

        // TipoCancha
        modelBuilder.Entity<TipoCancha>(b =>
        {
            b.ToTable("tipocancha");
            b.Property(x => x.Id).HasColumnName("id");
            b.Property(x => x.Nombre).HasColumnName("nombre").HasMaxLength(50).IsRequired();
            b.HasIndex(x => x.Nombre).IsUnique();
        });

        // Cancha
        modelBuilder.Entity<Cancha>(b =>
        {
            b.ToTable("cancha");
            b.Property(x => x.Id).HasColumnName("id");
            b.Property(x => x.ComplejoId).HasColumnName("complejoid");
            b.Property(x => x.TipoCanchaId).HasColumnName("tipocanchaid");
            b.Property(x => x.Nombre).HasColumnName("nombre").HasMaxLength(100).IsRequired();
            b.Property(x => x.PrecioHora).HasColumnName("preciohora").HasColumnType("decimal(10,2)");
            b.Property(x => x.Estado).HasColumnName("estado").HasDefaultValue(true);
            b.Property(x => x.Descripcion).HasColumnName("descripcion");
            b.HasOne(x => x.Complejo)
             .WithMany(c => c.Canchas)
             .HasForeignKey(x => x.ComplejoId)
             .OnDelete(DeleteBehavior.Cascade);

            b.HasOne(x => x.TipoCancha)
             .WithMany(t => t.Canchas)
             .HasForeignKey(x => x.TipoCanchaId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        // HorarioCancha
        modelBuilder.Entity<HorarioCancha>(b =>
        {
            b.ToTable("horario_cancha");
            b.Property(x => x.Id).HasColumnName("id");
            b.Property(x => x.CanchaId).HasColumnName("canchaid");
            b.Property(x => x.DiaSemana).HasColumnName("dia_semana").IsRequired();
            b.Property(x => x.HoraInicio).HasColumnName("hora_inicio").IsRequired();
            b.Property(x => x.HoraFin).HasColumnName("hora_fin").IsRequired();
            b.Property(x => x.Disponible).HasColumnName("disponible").HasDefaultValue(true);
    
            b.HasOne(x => x.Cancha)
             .WithMany(c => c.HorariosCanchas)
             .HasForeignKey(x => x.CanchaId)
             .OnDelete(DeleteBehavior.Cascade);
 
            b.HasIndex(x => new { x.CanchaId, x.DiaSemana, x.HoraInicio, x.HoraFin })
     .IsUnique();
        });

        // Reserva
        modelBuilder.Entity<Reserva>(b =>
        {
            b.ToTable("reserva");
            b.Property(x => x.Id).HasColumnName("id");
            b.Property(x => x.UsuarioId).HasColumnName("usuarioid");
            b.Property(x => x.ComplejoId).HasColumnName("complejoid");
            b.Property(x => x.CanchaId).HasColumnName("canchaid");
            b.Property(x => x.Fecha).HasColumnName("fecha").IsRequired();
            b.Property(x => x.FechaFin).HasColumnName("fechafin").IsRequired();
            b.Property(x => x.MedioPago).HasColumnName("mediopago").HasMaxLength(50);
            b.Property(x => x.MontoTotal).HasColumnName("montototal").HasColumnType("decimal(10,2)");
            b.Property(x => x.EstadoPago).HasColumnName("estadopago").HasConversion<string>().HasMaxLength(20).HasDefaultValue(EstadoPago.pendiente);
            b.Property(x => x.Confirmada).HasColumnName("confirmada").HasDefaultValue(false);
            b.Property(x => x.FechaReserva).HasColumnName("fechareserva").HasDefaultValueSql("CURRENT_TIMESTAMP");

            b.HasOne(x => x.Usuario)
             .WithMany(u => u.Reservas)
             .HasForeignKey(x => x.UsuarioId)
             .OnDelete(DeleteBehavior.Restrict);

            b.HasOne(x => x.Complejo)
             .WithMany(c => c.Reservas)
             .HasForeignKey(x => x.ComplejoId)
             .OnDelete(DeleteBehavior.Restrict);

            b.HasOne(x => x.Cancha)
             .WithMany(c => c.Reservas)
             .HasForeignKey(x => x.CanchaId)
             .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
