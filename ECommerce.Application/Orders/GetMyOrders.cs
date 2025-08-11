using ECommerce.Application.Abstractions;
using ECommerce.Application.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Application.Orders;

public record GetMyOrdersQuery() : IRequest<List<OrderDto>>;

public class GetMyOrdersHandler(IAppDbContext db, ICurrentUser user)
    : IRequestHandler<GetMyOrdersQuery, List<OrderDto>>
{
    public async Task<List<OrderDto>> Handle(GetMyOrdersQuery req, CancellationToken ct)
        => await db.Orders
            .Where(o => o.UserId == user.UserId)
            .OrderByDescending(o => o.CreatedAt)
            .Select(o => new OrderDto(o.Id, o.CreatedAt, o.Total))
            .ToListAsync(ct);
}
