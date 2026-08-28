using LaReservaBackend.Application.Reservas.Commands;
using LaReservaBackend.Application.Reservas.Queries;
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

    [HttpGet("HorariosDisponiblesComplejo")]
    public async Task<IActionResult> GetHorariosDisponiblesComplejo([FromQuery] int ComplejoId, CancellationToken cancellationToken)
    {
        var request = new GetHorariosDisponibles(ComplejoId);
        var response = await _mediator.Send(request, cancellationToken);

        return Ok(response);
    }

    [HttpGet("UserReservations")]
    public async Task<IActionResult> GetUserReservations([FromQuery] long UsuarioId, CancellationToken cancellationToken)
    {
        var request = new GetUserReservations(UsuarioId);
        var response = await _mediator.Send(request, cancellationToken);

        return Ok(response);
    }

    [HttpDelete("CancelReservation/{id}")]
    public async Task<IActionResult> CancelReservation(long id, CancellationToken cancellationToken)
    {
        var command = new CancelReservationCommand(id);
        var result = await _mediator.Send(command, cancellationToken);

        if (!result) return NotFound();

        return NoContent();
    }

    [HttpPost("CreateReserva")]
    public async Task<IActionResult> CreateReserva([FromBody] CreateReservaCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);

        if (result == -1)
        {
            return BadRequest(new { message = "El horario ya no está disponible." });
        }

        return Ok(new { id = result });
    }

    [HttpPost("BlockHorario")]
    public async Task<IActionResult> BlockHorario([FromBody] BlockHorarioCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);

        if (result == -1)
        {
            return BadRequest(new { message = "El horario ya no está disponible o existe una reserva." });
        }

        return Ok(new { id = result });
    }

    [HttpGet("GetNext")]
    public async Task<IActionResult> GetNext([FromQuery] long UsuarioId, CancellationToken cancellationToken)
    {
        var request = new GetNextReserva(UsuarioId);
        var response = await _mediator.Send(request, cancellationToken);

        return Ok(response);
    }
}
