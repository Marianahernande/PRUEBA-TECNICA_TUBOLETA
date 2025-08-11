using ECommerce.Application.Categories;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public Task<List<CategoryDto>> GetAll() => mediator.Send(new GetCategoriesQuery());

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public Task<CategoryDto> Create([FromBody] CreateCategoryCommand cmd) => mediator.Send(cmd);

    [Authorize(Roles = "Admin")]
    [HttpPut("{id:int}")]
    public Task<CategoryDto> Update(int id, [FromBody] UpdateCategoryCommand body)
        => mediator.Send(body with { Id = id });

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await mediator.Send(new DeleteCategoryCommand(id));
        return NoContent();
    }
}
