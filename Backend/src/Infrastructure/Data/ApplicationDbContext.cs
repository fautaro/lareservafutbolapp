using System.Reflection;
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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Tablas y restricciones
        modelBuilder.Entity<TipoUsuario>(b =>
        {
            b.ToTable("TipoUsuario");
            b.HasIndex(x => x.Nombre).IsUnique();
            b.Property(x => x.Nombre).HasMaxLength(20).IsRequired();
        });

        modelBuilder.Entity<Usuario>(b =>
        {
            b.ToTable("Usuario");
            b.HasIndex(x => x.Auth0Id).IsUnique();
            b.HasIndex(x => x.Email).IsUnique();

            b.Property(x => x.Auth0Id).HasMaxLength(128).IsRequired();
            b.Property(x => x.Nombre).HasMaxLength(100).IsRequired();
            b.Property(x => x.Email).HasMaxLength(100).IsRequired();
            b.Property(x => x.Telefono).HasMaxLength(20);

            b.Property(x => x.FechaRegistro)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            b.HasOne(x => x.TipoUsuario)
             .WithMany(t => t.Usuarios)
             .HasForeignKey(x => x.TipoUsuarioId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Ciudad>(b =>
        {
            b.ToTable("Ciudad");
            b.HasIndex(x => x.Nombre).IsUnique();
            b.Property(x => x.Nombre).HasMaxLength(100).IsRequired();
        });

        modelBuilder.Entity<Deporte>(b =>
        {
            b.ToTable("Deporte");
            b.Property(x => x.Nombre).HasMaxLength(100).IsRequired();
            b.Property(x => x.Icon).HasMaxLength(200);
            b.Property(x => x.BgClass).HasMaxLength(100);
            b.Property(x => x.TextClass).HasMaxLength(100);
        });

        modelBuilder.Entity<Complejo>(b =>
        {
            b.ToTable("Complejo");

            b.Property(x => x.Nombre).HasMaxLength(100).IsRequired();
            b.Property(x => x.Direccion).HasMaxLength(200);
            b.Property(x => x.Precio).HasColumnType("decimal(10,2)");
            b.Property(x => x.Imagen);
            b.Property(x => x.Estado).HasDefaultValue(true);
            b.Property(x => x.Categoria).HasMaxLength(100);
            b.Property(x => x.DeportePillBg).HasMaxLength(100);
            b.Property(x => x.DeportePillText).HasMaxLength(100);

            b.Property(x => x.FechaCreacion)
             .HasDefaultValueSql("CURRENT_TIMESTAMP");

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

        modelBuilder.Entity<TipoCancha>(b =>
        {
            b.ToTable("TipoCancha");
            b.HasIndex(x => x.Nombre).IsUnique();
            b.Property(x => x.Nombre).HasMaxLength(50).IsRequired();
        });

        modelBuilder.Entity<Cancha>(b =>
        {
            b.ToTable("Cancha");
            b.Property(x => x.Nombre).HasMaxLength(100).IsRequired();
            b.Property(x => x.PrecioHora).HasColumnType("decimal(10,2)");
            b.Property(x => x.Estado).HasDefaultValue(true);

            b.HasOne(x => x.Complejo)
             .WithMany(c => c.Canchas)
             .HasForeignKey(x => x.ComplejoId)
             .OnDelete(DeleteBehavior.Cascade);

            b.HasOne(x => x.TipoCancha)
             .WithMany(t => t.Canchas)
             .HasForeignKey(x => x.TipoCanchaId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Reserva>(b =>
        {
            b.ToTable("Reserva");
            b.Property(x => x.Fecha).IsRequired();
            b.Property(x => x.FechaFin).IsRequired();
            b.Property(x => x.MedioPago).HasMaxLength(50);
            b.Property(x => x.MontoTotal).HasColumnType("decimal(10,2)");

            b.Property(x => x.FechaReserva)
             .HasDefaultValueSql("CURRENT_TIMESTAMP");

            // Mapear enum EstadoPago como string (varchar)
            b.Property(x => x.EstadoPago)
             .HasConversion<string>()
             .HasMaxLength(20)
             .HasDefaultValue(EstadoPago.pendiente);

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
