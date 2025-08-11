using ECommerce.Application.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Pricing;

public class FakeCompetitorClient(IAppDbContext db) : ICompetitorClient
{
    private static readonly Random _rng = new();

    public async Task<decimal> GetPriceAsync(int productId, CancellationToken ct)
    {
        var p = await db.Products.AsNoTracking().FirstAsync(x => x.Id == productId, ct);
        // Simulación: precio de la competencia ±10%
        var factor = 0.9m + (decimal)_rng.NextDouble() * 0.2m;
        return Math.Round(p.Price * factor, 2);
    }
}
