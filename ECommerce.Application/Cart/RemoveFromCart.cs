using ECommerce.Application.Abstractions;
using ECommerce.Application.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Application.Cart;

public record RemoveFromCartCommand(int CartItemId) : IRequest;

public class RemoveFromCartHandler(IAppDbContext db, ICurrentUser user)
    : IRequestHandler<RemoveFromCartCommand>
{
    public async Task Handle(RemoveFromCartCommand req, CancellationToken ct)
    {
        var item = await db.CartItems
            .FirstOrDefaultAsync(i => i.Id == req.CartItemId && i.UserId == user.UserId, ct)
            ?? throw new KeyNotFoundException("Item no encontrado.");

        db.CartItems.Remove(item);
        await db.SaveChangesAsync(ct);
    }
}
