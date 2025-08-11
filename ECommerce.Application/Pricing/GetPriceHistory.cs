using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ECommerce.Application.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Application.Pricing
{
    public record GetPriceHistoryQuery(int ProductId, int Days = 30)
        : IRequest<List<PricePointDto>>;

    public record PricePointDto(DateTime AppliedAt, decimal OldPrice, decimal NewPrice);

    public class GetPriceHistoryHandler(IAppDbContext db)
        : IRequestHandler<GetPriceHistoryQuery, List<PricePointDto>>
    {
        public async Task<List<PricePointDto>> Handle(GetPriceHistoryQuery req, CancellationToken ct)
        {
            var since = DateTime.UtcNow.AddDays(-req.Days);

            return await db.PriceHistories
                .Where(p => p.ProductId == req.ProductId && p.AppliedAt >= since)
                .OrderBy(p => p.AppliedAt)
                .Select(p => new PricePointDto(p.AppliedAt, p.OldPrice, p.NewPrice))
                .ToListAsync(ct);
        }
    }
}
