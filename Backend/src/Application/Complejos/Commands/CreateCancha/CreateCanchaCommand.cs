using LaReservaBackend.Application.Common.Interfaces;
using LaReservaBackend.Domain.Entities;
using MediatR;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System;

namespace LaReservaBackend.Application.Complejos.Commands.CreateCancha;

public class CreateCanchaCommand : IRequest<long>
{
    public long ComplejoId { get; set; }
    public long TipoCanchaId { get; set; }
    [Required]
    public string Nombre { get; set; } = null!;
    public decimal? PrecioHora { get; set; }
    public string? Descripcion { get; set; }
}

public class CreateCanchaCommandHandler : IRequestHandler<CreateCanchaCommand, long>
{
    private readonly IApplicationDbContext _context;

    public CreateCanchaCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<long> Handle(CreateCanchaCommand request, CancellationToken cancellationToken)
    {
        var complejo = await _context.Complejos.FirstOrDefaultAsync(c => c.Id == request.ComplejoId, cancellationToken);
        if (complejo == null) throw new Exception("Complejo no encontrado");

        var entity = new Cancha
        {
            ComplejoId = request.ComplejoId,
            TipoCanchaId = request.TipoCanchaId,
            Nombre = request.Nombre,
            PrecioHora = request.PrecioHora,
            Descripcion = request.Descripcion,
            Estado = true
        };

        _context.Canchas.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
