using LaReservaBackend.Application.Usuarios.Commands.BajaUsuario;
using LaReservaBackend.Application.Usuarios.Commands.CreateUsuario;
using LaReservaBackend.Application.Usuarios.Commands.ReactivarUsuario;
using LaReservaBackend.Application.Usuarios.Queries;
using LaReservaBackend.Application.Usuarios.Queries.GetUsuarios;
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

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var query = new GetUsuariosQuery();
        var response = await _mediator.Send(query, cancellationToken);
        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserProfile(long id, CancellationToken cancellationToken)
    {
        var query = new GetUserProfile(id);
        var response = await _mediator.Send(query, cancellationToken);

        if (response == null) return NotFound();

        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUsuarioCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var id = await _mediator.Send(command, cancellationToken);
            return CreatedAtAction(nameof(GetUserProfile), new { id }, new { id });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LaReservaBackend.Application.Usuarios.Commands.LoginUsuario.LoginUsuarioCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var response = await _mediator.Send(command, cancellationToken);
            return Ok(response);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
    }

    [HttpPost("cambiar-password")]
    public async Task<IActionResult> CambiarPassword([FromBody] LaReservaBackend.Application.Usuarios.Commands.CambiarPassword.CambiarPasswordCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(new { success = result, message = "Contraseña actualizada exitosamente." });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id}/baja")]
    public async Task<IActionResult> DarDeBaja(long id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new BajaUsuarioCommand(id), cancellationToken);
        if (!result) return NotFound(new { message = "Usuario no encontrado." });
        return Ok(new { success = true, message = "Usuario dado de baja exitosamente." });
    }

    [HttpPut("{id}/reactivar")]
    public async Task<IActionResult> Reactivar(long id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new ReactivarUsuarioCommand(id), cancellationToken);
        if (!result) return NotFound(new { message = "Usuario no encontrado." });
        return Ok(new { success = true, message = "Usuario reactivado exitosamente." });
    }
}
