using ECommerce.Application.Abstractions;
using ECommerce.Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Application.Products;

public record CreateProductCommand(string Name, string Sku, decimal Price, int CategoryId) : IRequest<ProductDto>;

public class CreateProductValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Sku).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Price).GreaterThan(0);
        RuleFor(x => x.CategoryId).GreaterThan(0);
    }
}

public class CreateProductHandler(IAppDbContext db) : IRequestHandler<CreateProductCommand, ProductDto>
{
    public async Task<ProductDto> Handle(CreateProductCommand req, CancellationToken ct)
    {
        var cat = await db.Categories.FindAsync(new object?[] { req.CategoryId }, ct)
                  ?? throw new KeyNotFoundException("Categoría no encontrada.");

        var skuTaken = await db.Products.AnyAsync(p => p.Sku == req.Sku, ct);
        if (skuTaken) throw new InvalidOperationException("SKU ya utilizado.");

        var p = new Product { Name = req.Name.Trim(), Sku = req.Sku.Trim(), Price = req.Price, CategoryId = req.CategoryId };
        db.Products.Add(p);
        await db.SaveChangesAsync(ct);

        return new ProductDto(p.Id, p.Name, p.Sku, p.Price, p.CategoryId, cat.Name);
    }
}
