namespace ECommerce.Infrastructure.Pricing;

public interface ICompetitorClient
{
    Task<decimal> GetPriceAsync(int productId, CancellationToken ct);
}
