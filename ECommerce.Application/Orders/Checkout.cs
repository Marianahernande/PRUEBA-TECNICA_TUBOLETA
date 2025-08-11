using ECommerce.Application.Abstractions;
using ECommerce.Application.Common;
using ECommerce.Application.Notifications;
using ECommerce.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace ECommerce.Application.Orders;

public record CheckoutCommand() : IRequest<OrderDto>;

public class CheckoutHandler(IAppDbContext db, ICurrentUser user, INotificationService notify)
    : IRequestHandler<CheckoutCommand, OrderDto>
{
    public async Task<OrderDto> Handle(CheckoutCommand req, CancellationToken ct)
    {
        var cart = await db.CartItems
            .Where(i => i.UserId == user.UserId)
            .Include(i => i.Product)
            .ToListAsync(ct);

        if (cart.Count == 0) 
            throw new InvalidOperationException("El carrito está vacío.");

        var order = new Order
        {
            UserId = user.UserId,
            CreatedAt = DateTime.UtcNow,
            Total = cart.Sum(i => i.Product!.Price * i.Quantity),
            Items = cart.Select(i => new OrderItem
            {
                ProductId = i.ProductId,
                Quantity  = i.Quantity,
                UnitPrice = i.Product!.Price
            }).ToList()
        };

        db.Orders.Add(order);
        db.CartItems.RemoveRange(cart);
        await db.SaveChangesAsync(ct);

        // Notificación: un solo string + ct firma de INotificationService...
        var totalFormat = order.Total.ToString("C", CultureInfo.GetCultureInfo("es-CO"));
        var mensaje = $"Pedido creado: #{order.Id} por {totalFormat}";
        await notify.SendAsync(mensaje, ct);

        return new OrderDto(order.Id, order.CreatedAt, order.Total);
    }
}
