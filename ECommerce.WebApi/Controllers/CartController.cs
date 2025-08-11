using ECommerce.Application.Cart;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CartController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public Task<List<CartItemDto>> Get() => mediator.Send(new GetCartQuery());

    [HttpPost("items")]
    public async Task<IActionResult> Add([FromBody] AddToCartCommand cmd)
    {
        await mediator.Send(cmd);
        return NoContent();
    }

    [HttpDelete("items/{cartItemId:int}")]
    public async Task<IActionResult> Remove(int cartItemId)
    {
        await mediator.Send(new RemoveFromCartCommand(cartItemId));
        return NoContent();
    }
}
