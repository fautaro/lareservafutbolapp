using Microsoft.AspNetCore.Mvc;
using MediatR;
using LaReservaBackend.Application.Complejos.Commands.CreateHorarioCancha;
using LaReservaBackend.Application.Complejos.Commands.DeleteHorarioCancha;
using LaReservaBackend.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace LaReservaApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CanchasController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IApplicationDbContext _context;

    public CanchasController(IMediator mediator, IApplicationDbContext context)
    {
        _mediator = mediator;
        _context = context;
    }

    [HttpGet("{canchaId}/horarios")]
    public async Task<IActionResult> GetHorarios(long canchaId, CancellationToken cancellationToken)
    {
        var horarios = await _context.HorariosCanchas
            .Where(h => h.CanchaId == canchaId)
            .OrderBy(h => h.DiaSemana)
            .ThenBy(h => h.HoraInicio)
            .ToListAsync(cancellationToken);

        return Ok(horarios);
    }

    [HttpPost("{canchaId}/horarios")]
    public async Task<IActionResult> CreateHorarioCancha(long canchaId, [FromBody] CreateHorarioCanchaCommand command, CancellationToken cancellationToken)
    {
        command.CanchaId = canchaId;
        // Obtenemos complejoId si no viene en el command
        if(command.ComplejoId == 0)
        {
             var cancha = await _context.Canchas.FindAsync(new object[] { canchaId }, cancellationToken);
             if(cancha != null) command.ComplejoId = cancha.ComplejoId;
        }

        var result = await _mediator.Send(command, cancellationToken);
        return Ok(new { id = result });
    }

    [HttpDelete("{canchaId}/horarios/{horarioId}")]
    public async Task<IActionResult> DeleteHorarioCancha(long canchaId, long horarioId, CancellationToken cancellationToken)
    {
        var cancha = await _context.Canchas.FindAsync(new object[] { canchaId }, cancellationToken);
        var request = new DeleteHorarioCanchaCommand
        {
            ComplejoId = cancha?.ComplejoId ?? 0,
            CanchaId = canchaId,
            HorarioId = horarioId
        };
        var result = await _mediator.Send(request, cancellationToken);
        if (!result) return NotFound();
        return Ok();
    }

    [HttpPut("{canchaId}/estado")]
    public async Task<IActionResult> UpdateEstado(long canchaId, [FromBody] LaReservaBackend.Application.Complejos.Commands.UpdateCanchaEstado.UpdateCanchaEstadoCommand command, CancellationToken cancellationToken)
    {
        command.CanchaId = canchaId;
        var result = await _mediator.Send(command, cancellationToken);
        if (!result) return Forbid();
        return Ok();
    }

    [HttpPut("{canchaId}/precio")]
    public async Task<IActionResult> UpdatePrecio(long canchaId, [FromBody] LaReservaBackend.Application.Complejos.Commands.UpdateCanchaPrecio.UpdateCanchaPrecioCommand command, CancellationToken cancellationToken)
    {
        command.CanchaId = canchaId;
        var result = await _mediator.Send(command, cancellationToken);
        if (!result) return Forbid();
        return Ok();
    }
}
