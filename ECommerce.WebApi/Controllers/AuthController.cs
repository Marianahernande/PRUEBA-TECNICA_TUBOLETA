using ECommerce.Application.Auth;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.WebApi.Controllers;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;
    public AuthController(IMediator mediator) => _mediator = mediator;

    public record RegisterRequest(string Email, string Password, string? Role);
    public record LoginRequest(string Email, string Password);

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterRequest req)
    {
        var token = await _mediator.Send(new RegisterUserCommand
        {
            Email = req.Email,
            Password = req.Password,
            Role = req.Role
        });
        return Ok(new { token });
    }

    [HttpPost("register-admin")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> RegisterAdmin([FromBody] RegisterRequest req)
    {
        var token = await _mediator.Send(new RegisterUserCommand
        {
            Email = req.Email,
            Password = req.Password,
            Role = "Admin"
        });
        return Ok(new { token });
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginRequest req)
    {
        try
        {
            var token = await _mediator.Send(new LoginQuery(req.Email, req.Password));
            return Ok(new { token });
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
    }
}
