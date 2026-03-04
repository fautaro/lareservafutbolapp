using LaReservaBackend.Application.Usuarios.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LaReservaApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsuarioController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsuarioController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserProfile(long id, CancellationToken cancellationToken)
    {
        var query = new GetUserProfile(id);
        var response = await _mediator.Send(query, cancellationToken);

        if (response == null) return NotFound();

        return Ok(response);
    }
}
