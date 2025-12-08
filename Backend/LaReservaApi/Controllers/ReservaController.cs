using LaReservaBackend.Application.Complejos.Queries.GetComplejos;
using LaReservaBackend.Application.Reservas.Queries;
using LaReservaBackend.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LaReservaApi.Controllers;


[ApiController]
[Route("api/[controller]")]
public class ReservaController : Controller
{
    private readonly IMediator _mediator;

    public ReservaController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("HorariosDisponiblesCancha")]
    public async Task<IActionResult> GetHorariosDisponiblesCancha([FromQuery] int CanchaId, CancellationToken cancellationToken)
    {
        var request = new GetHorariosDisponibles(CanchaId);
        var response = await _mediator.Send(request, cancellationToken);

        return Ok(response);

    }
}
