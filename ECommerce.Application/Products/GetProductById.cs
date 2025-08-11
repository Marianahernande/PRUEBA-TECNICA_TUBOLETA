using ECommerce.Application.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Application.Products;

public record GetProductByIdQuery(int Id) : IRequest<ProductDto>;

public class GetProductByIdHandler(IAppDbContext db) : IRequestHandler<GetProductByIdQuery, ProductDto>
{
    public async Task<ProductDto> Handle(GetProductByIdQuery req, CancellationToken ct)
    {
        var p = await db.Products.Include(x => x.Category)
                    .FirstOrDefaultAsync(x => x.Id == req.Id, ct)
                ?? throw new KeyNotFoundException("Producto no encontrado.");

        return new ProductDto(p.Id, p.Name, p.Sku, p.Price, p.CategoryId, p.Category!.Name);
    }
}
