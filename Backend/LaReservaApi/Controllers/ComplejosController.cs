using LaReservaBackend.Application.Complejos.Queries.GetComplejos;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

    [HttpGet("{id}/estadisticas-dia")]
    public async Task<IActionResult> GetEstadisticasDia(long id, [FromQuery] DateTime fecha, [FromQuery] long usuarioId, CancellationToken cancellationToken = default)
    {
        var request = new LaReservaBackend.Application.Complejos.Queries.GetEstadisticasDia.GetEstadisticasDiaComplejoQuery(id, fecha, usuarioId);
        var response = await _mediator.Send(request, cancellationToken);

        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> CreateComplejo([FromBody] LaReservaBackend.Application.Complejos.Commands.CreateComplejo.CreateComplejoCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(new { id = result });
    }

    [HttpGet("{id}/config")]
    public async Task<IActionResult> GetConfig(long id, [FromQuery] long? usuarioId, CancellationToken cancellationToken)
    {
        var request = new LaReservaBackend.Application.Complejos.Queries.GetComplejoConfig.GetComplejoConfigQuery { ComplejoId = id, UsuarioId = usuarioId };
        var response = await _mediator.Send(request, cancellationToken);
        
        if (response == null) return NotFound();
        return Ok(response);
    }

    [HttpPost("{id}/canchas")]
    public async Task<IActionResult> CreateCancha(long id, [FromBody] LaReservaBackend.Application.Complejos.Commands.CreateCancha.CreateCanchaCommand command, CancellationToken cancellationToken)
    {
        command.ComplejoId = id;
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(new { id = result });
    }

    [HttpPut("{id}/estado")]
    public async Task<IActionResult> UpdateEstado(long id, [FromBody] LaReservaBackend.Application.Complejos.Commands.UpdateComplejoEstado.UpdateComplejoEstadoCommand command, CancellationToken cancellationToken)
    {
        command.ComplejoId = id;
        var result = await _mediator.Send(command, cancellationToken);
        if (!result) return Forbid();
        return Ok();
    }

    [HttpPut("{id}/imagen")]
    public async Task<IActionResult> UpdateImagen(long id, [FromBody] LaReservaBackend.Application.Complejos.Commands.UpdateComplejoImagen.UpdateComplejoImagenCommand command, CancellationToken cancellationToken)
    {
        command.ComplejoId = id;
        var result = await _mediator.Send(command, cancellationToken);
        
        if (result.IsForbidden)
        {
            return Forbid();
        }
        
        if (!result.Success)
        {
            return BadRequest(new { message = result.ErrorMessage });
        }
        
        return Ok();
    }

    [HttpPut("{id}/direccion")]
    public async Task<IActionResult> UpdateDireccion(long id, [FromBody] LaReservaBackend.Application.Complejos.Commands.UpdateComplejoDireccion.UpdateComplejoDireccionCommand command, CancellationToken cancellationToken)
    {
        command.ComplejoId = id;
        var result = await _mediator.Send(command, cancellationToken);
        
        if (result.IsForbidden)
        {
            return Forbid();
        }
        
        if (!result.Success)
        {
            return BadRequest(new { message = result.ErrorMessage });
        }
        
        return Ok();
    }


    [HttpGet("debug-reservas")]
    public async Task<IActionResult> DebugReservas([FromServices] LaReservaBackend.Application.Common.Interfaces.IApplicationDbContext context)
    {
        var list = await EntityFrameworkQueryableExtensions.ToListAsync(context.Reservas);
        return Ok(list);
    }
}
