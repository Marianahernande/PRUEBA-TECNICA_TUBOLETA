using ECommerce.Application.Abstractions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Application.Products;

public record UpdateProductCommand(int Id, string Name, string Sku, decimal Price, int CategoryId) : IRequest<ProductDto>;

public class UpdateProductValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Sku).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Price).GreaterThan(0);
        RuleFor(x => x.CategoryId).GreaterThan(0);
    }
}

public class UpdateProductHandler(IAppDbContext db) : IRequestHandler<UpdateProductCommand, ProductDto>
{
    public async Task<ProductDto> Handle(UpdateProductCommand req, CancellationToken ct)
    {
        var p = await db.Products.Include(x => x.Category)
                    .FirstOrDefaultAsync(x => x.Id == req.Id, ct)
                ?? throw new KeyNotFoundException("Producto no encontrado.");

        var skuTaken = await db.Products.AnyAsync(x => x.Id != req.Id && x.Sku == req.Sku, ct);
        if (skuTaken) throw new InvalidOperationException("SKU ya utilizado.");

        var cat = await db.Categories.FindAsync(new object?[] { req.CategoryId }, ct)
                  ?? throw new KeyNotFoundException("Categoría no encontrada.");

        p.Name = req.Name.Trim();
        p.Sku = req.Sku.Trim();
        p.Price = req.Price;
        p.CategoryId = req.CategoryId;

        await db.SaveChangesAsync(ct);

        return new ProductDto(p.Id, p.Name, p.Sku, p.Price, p.CategoryId, cat.Name);
    }
}
