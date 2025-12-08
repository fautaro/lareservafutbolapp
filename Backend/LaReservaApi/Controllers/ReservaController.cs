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

    [HttpGet]
    public async Task<IActionResult> GetHorariosDisponiblesCancha([FromQuery] int CanchaId, CancellationToken cancellationToken)
    {
        await Task.Delay(100);
        return Ok();

    }
}
