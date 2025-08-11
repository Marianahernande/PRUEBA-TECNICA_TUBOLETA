using ECommerce.Application.Abstractions;
using ECommerce.Application.Common;
using ECommerce.Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Application.Cart;

public record AddToCartCommand(int ProductId, int Quantity) : IRequest;

public class AddToCartValidator : AbstractValidator<AddToCartCommand>
{
    public AddToCartValidator()
    {
        RuleFor(x => x.ProductId).GreaterThan(0);
        RuleFor(x => x.Quantity).GreaterThan(0);
    }
}

public class AddToCartHandler(IAppDbContext db, ICurrentUser user) : IRequestHandler<AddToCartCommand>
{
    public async Task Handle(AddToCartCommand req, CancellationToken ct)
    {
        _ = await db.Products.FirstOrDefaultAsync(p => p.Id == req.ProductId, ct)
            ?? throw new KeyNotFoundException("Producto no encontrado.");

        var item = await db.CartItems
            .FirstOrDefaultAsync(i => i.UserId == user.UserId && i.ProductId == req.ProductId, ct);

        if (item is null)
            db.CartItems.Add(new CartItem { UserId = user.UserId, ProductId = req.ProductId, Quantity = req.Quantity });
        else
            item.Quantity += req.Quantity;

        await db.SaveChangesAsync(ct);
    }
}
