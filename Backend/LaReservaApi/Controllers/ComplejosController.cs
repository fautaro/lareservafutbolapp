using LaReservaApi.Models.Complejos;
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
        var request = new GetComplejosRequest(complejoId ?? 0, deporteId ?? 0);
        var response = await _mediator.Send(request, cancellationToken);

        return Ok(response);
    }
}
