using ECommerce.Application.Products;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IMediator _mediator;
    public ProductsController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public Task<List<ProductDto>> Get() => _mediator.Send(new GetProductsQuery());

    [HttpGet("{id:int}")]
    public Task<ProductDto> GetById(int id) => _mediator.Send(new GetProductByIdQuery(id));

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public Task<ProductDto> Create([FromBody] CreateProductCommand cmd) => _mediator.Send(cmd);

    [Authorize(Roles = "Admin")]
    [HttpPut("{id:int}")]
    public Task<ProductDto> Update(int id, [FromBody] UpdateProductCommand body)
        => _mediator.Send(body with { Id = id });

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _mediator.Send(new DeleteProductCommand(id));
        return NoContent();
    }
}
