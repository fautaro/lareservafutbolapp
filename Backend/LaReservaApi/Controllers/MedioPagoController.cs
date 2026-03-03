using LaReservaBackend.Application.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LaReservaApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MedioPagoController : ControllerBase
{
    private readonly IApplicationDbContext _context;

    public MedioPagoController(IApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var medios = await _context.MedioPagos
            .Where(x => x.Activo)
            .OrderBy(x => x.Orden)
            .Select(x => new
            {
                x.Id,
                x.Nombre,
                x.Codigo,
                x.Icono,
                x.RequiereComprobante,
                x.Descripcion
            })
            .ToListAsync();

        return Ok(medios);
    }
}
