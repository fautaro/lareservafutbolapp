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

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] GetComplejosRequestDTO? input, CancellationToken cancellationToken)
    {
        var request = new GetComplejosRequest(input?.ComplejoId ?? 0, input?.DeporteId ?? 0);
        var response = await _mediator.Send(request, cancellationToken);

        return Ok(response);
    }
}
