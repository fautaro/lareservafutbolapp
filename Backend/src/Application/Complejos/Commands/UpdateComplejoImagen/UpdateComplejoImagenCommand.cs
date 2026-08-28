using MediatR;
using LaReservaBackend.Application.Common.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace LaReservaBackend.Application.Complejos.Commands.UpdateComplejoImagen;

public class UpdateComplejoImagenCommand : IRequest<UpdateComplejoImagenResult>
{
    public long ComplejoId { get; set; }
    public long UsuarioId { get; set; }
    public string ImagenUrl { get; set; } = null!;
}

public class UpdateComplejoImagenResult
{
    public bool Success { get; set; }
    public bool IsForbidden { get; set; }
    public string? ErrorMessage { get; set; }
}

public class UpdateComplejoImagenCommandHandler : IRequestHandler<UpdateComplejoImagenCommand, UpdateComplejoImagenResult>
{
    private readonly IApplicationDbContext _context;

    public UpdateComplejoImagenCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<UpdateComplejoImagenResult> Handle(UpdateComplejoImagenCommand request, CancellationToken cancellationToken)
    {
        var complejo = await _context.Complejos.FindAsync(new object[] { request.ComplejoId }, cancellationToken);
        if (complejo == null || complejo.DuenoId != request.UsuarioId)
        {
            return new UpdateComplejoImagenResult { Success = false, IsForbidden = true, ErrorMessage = "No tienes permisos para modificar este complejo." };
        }

        if (string.IsNullOrWhiteSpace(request.ImagenUrl))
        {
            return new UpdateComplejoImagenResult { Success = false, ErrorMessage = "La URL de la imagen es requerida." };
        }

        if (!Uri.TryCreate(request.ImagenUrl, UriKind.Absolute, out var uri))
        {
            return new UpdateComplejoImagenResult { Success = false, ErrorMessage = "La URL provista no es válida. Debe comenzar con http:// o https://." };
        }

        var path = uri.AbsolutePath;
        if (!path.EndsWith(".png", StringComparison.OrdinalIgnoreCase) &&
            !path.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) &&
            !path.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase))
        {
            return new UpdateComplejoImagenResult { Success = false, ErrorMessage = "La imagen debe ser de formato PNG, JPG o JPEG." };
        }

        complejo.Imagen = request.ImagenUrl;
        await _context.SaveChangesAsync(cancellationToken);

        return new UpdateComplejoImagenResult { Success = true };
    }
}
