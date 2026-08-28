using LaReservaBackend.Application.ReferenceData.Queries.GetReferenceData;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LaReservaApi.Controllers;

[ApiController]
[Route("api/reference-data")]
public class ReferenceDataController : ControllerBase
{
    private readonly IMediator _mediator;

    public ReferenceDataController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new GetReferenceDataQuery(), cancellationToken);
        return Ok(response);
    }
}
