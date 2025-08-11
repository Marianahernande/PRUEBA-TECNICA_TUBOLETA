using ECommerce.Application.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Application.Orders;

public record GetOrderByIdQuery(int Id) : IRequest<OrderDetailsDto>;

public class GetOrderByIdHandler(IAppDbContext db) : IRequestHandler<GetOrderByIdQuery, OrderDetailsDto>
{
    public async Task<OrderDetailsDto> Handle(GetOrderByIdQuery req, CancellationToken ct)
    {
        var order = await db.Orders
            .Include(o => o.Items)
            .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(o => o.Id == req.Id, ct)
            ?? throw new KeyNotFoundException("Pedido no encontrado.");

        var items = order.Items
            .Select(i => new OrderItemDto(
                i.ProductId,
                i.Product?.Name ?? string.Empty,
                i.Quantity,
                i.UnitPrice,
                i.UnitPrice * i.Quantity))
            .ToList();

        return new OrderDetailsDto(order.Id, order.CreatedAt, order.Total, items);
    }
}
