using MediatR;
using LaReservaBackend.Application.Common.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace LaReservaBackend.Application.Complejos.Commands.UpdateComplejoDireccion;

public class UpdateComplejoDireccionCommand : IRequest<UpdateComplejoDireccionResult>
{
    public long ComplejoId { get; set; }
    public long UsuarioId { get; set; }
    public long CiudadId { get; set; }
    public string? Direccion { get; set; }
}

public class UpdateComplejoDireccionResult
{
    public bool Success { get; set; }
    public bool IsForbidden { get; set; }
    public string? ErrorMessage { get; set; }
}

public class UpdateComplejoDireccionCommandHandler : IRequestHandler<UpdateComplejoDireccionCommand, UpdateComplejoDireccionResult>
{
    private readonly IApplicationDbContext _context;

    public UpdateComplejoDireccionCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<UpdateComplejoDireccionResult> Handle(UpdateComplejoDireccionCommand request, CancellationToken cancellationToken)
    {
        var complejo = await _context.Complejos.FindAsync(new object[] { request.ComplejoId }, cancellationToken);
        if (complejo == null || complejo.DuenoId != request.UsuarioId)
        {
            return new UpdateComplejoDireccionResult { Success = false, IsForbidden = true, ErrorMessage = "No tienes permisos para modificar este complejo." };
        }

        // Validate that CiudadId corresponds to a valid Ciudad
        var ciudadExists = await _context.Ciudades.AnyAsync(c => c.Id == request.CiudadId, cancellationToken);
        if (!ciudadExists)
        {
            return new UpdateComplejoDireccionResult { Success = false, ErrorMessage = "La ciudad seleccionada no es válida." };
        }

        // Validate Direccion length (max 200)
        if (request.Direccion != null && request.Direccion.Length > 200)
        {
            return new UpdateComplejoDireccionResult { Success = false, ErrorMessage = "La dirección no puede exceder los 200 caracteres." };
        }

        complejo.CiudadId = request.CiudadId;
        complejo.Direccion = request.Direccion;
        await _context.SaveChangesAsync(cancellationToken);

        return new UpdateComplejoDireccionResult { Success = true };
    }
}
