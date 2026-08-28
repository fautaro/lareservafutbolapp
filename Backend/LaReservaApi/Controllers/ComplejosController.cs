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
    public async Task<IActionResult> Get([FromQuery] int? complejoId, [FromQuery] int? deporteId, CancellationToken cancellationToken)
    {
        var request = new GetComplejos(complejoId ?? 0, deporteId ?? 0);
        var response = await _mediator.Send(request, cancellationToken);

        return Ok(response);
    }

    [HttpGet("{id}/agenda")]
    public async Task<IActionResult> GetAgenda(long id, [FromQuery] DateTime fecha, [FromQuery] long UsuarioId, CancellationToken cancellationToken)
    {
        var request = new LaReservaBackend.Application.Complejos.Queries.GetAgendaComplejo.GetAgendaComplejoQuery(id, fecha, UsuarioId);
        var response = await _mediator.Send(request, cancellationToken);

        return Ok(response);
    }
}
