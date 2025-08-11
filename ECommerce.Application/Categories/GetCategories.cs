using ECommerce.Application.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Application.Categories;

public record GetCategoriesQuery() : IRequest<List<CategoryDto>>;

public class GetCategoriesHandler(IAppDbContext db) : IRequestHandler<GetCategoriesQuery, List<CategoryDto>>
{
    public async Task<List<CategoryDto>> Handle(GetCategoriesQuery req, CancellationToken ct)
        => await db.Categories
            .Select(c => new CategoryDto(c.Id, c.Name))
            .ToListAsync(ct);
}
