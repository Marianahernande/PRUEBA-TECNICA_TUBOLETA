using ECommerce.Application.Orders;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ECommerce.Application.Orders;

namespace ECommerce.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class OrdersController(IMediator mediator) : ControllerBase
{
    [HttpPost("checkout")]
    public Task<OrderDto> Checkout() => mediator.Send(new CheckoutCommand());

    [HttpGet("mine")]
    public Task<List<OrderDto>> MyOrders() => mediator.Send(new GetMyOrdersQuery());

    [HttpGet("{id:int}")]
    public Task<OrderDetailsDto> GetById(int id) => mediator.Send(new GetOrderByIdQuery(id));
}