using ECommerce.Application.Abstractions;
using ECommerce.Application.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Application.Cart;

public record GetCartQuery() : IRequest<List<CartItemDto>>;

public class GetCartHandler(IAppDbContext db, ICurrentUser user)
    : IRequestHandler<GetCartQuery, List<CartItemDto>>
{
    public async Task<List<CartItemDto>> Handle(GetCartQuery req, CancellationToken ct)
        => await db.CartItems
            .Where(i => i.UserId == user.UserId)
            .Include(i => i.Product)
            .Select(i => new CartItemDto(
                i.Id, i.ProductId, i.Product!.Name, i.Product.Price, i.Quantity, i.Product.Price * i.Quantity))
            .ToListAsync(ct);
}
