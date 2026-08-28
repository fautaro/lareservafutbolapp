using LaReservaBackend.Application.Complejos.Queries.GetComplejos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LaReservaApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ComplejosController : Controller
{
    private readonly IMediator _mediator;

    public ComplejosController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] int? complejoId, [FromQuery] int? deporteId, [FromQuery] long? duenoId, CancellationToken cancellationToken)
    {
        var request = new GetComplejos(complejoId ?? 0, deporteId ?? 0, duenoId);
        var response = await _mediator.Send(request, cancellationToken);

        return Ok(response);
    }

    [HttpGet("{id}/agenda")]
    public async Task<IActionResult> GetAgenda(long id, [FromQuery] DateTime fecha, [FromQuery] long UsuarioId, [FromQuery] bool soloReservas = false, CancellationToken cancellationToken = default)
    {
        var request = new LaReservaBackend.Application.Complejos.Queries.GetAgendaComplejo.GetAgendaComplejoQuery(id, fecha, UsuarioId, soloReservas);
        var response = await _mediator.Send(request, cancellationToken);

        return Ok(response);
    }

    [HttpGet("debug-reservas")]
    public async Task<IActionResult> DebugReservas([FromServices] LaReservaBackend.Application.Common.Interfaces.IApplicationDbContext context)
    {
        var list = await Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions.ToListAsync(context.Reservas);
        return Ok(list);
    }
}
