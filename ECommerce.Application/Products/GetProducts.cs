using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ECommerce.Application.Abstractions;

namespace ECommerce.Application.Products;

public record GetProductsQuery() : IRequest<List<ProductDto>>;

public class GetProductsHandler(IAppDbContext db, IMapper mapper)
    : IRequestHandler<GetProductsQuery, List<ProductDto>>
{
    public async Task<List<ProductDto>> Handle(GetProductsQuery request, CancellationToken ct)
        => await db.Products
            .Include(p => p.Category)
            .ProjectTo<ProductDto>(mapper.ConfigurationProvider)
            .ToListAsync(ct);
}
