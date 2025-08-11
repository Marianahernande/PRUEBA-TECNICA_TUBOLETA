using ECommerce.Domain.Entities;

namespace ECommerce.Application.Pricing;
public interface IPricingService
{
    Task<decimal> PreviewAsync(Product product, DateTime date, CancellationToken ct = default);
    Task<decimal> ApplyAsync(Product product, DateTime date, CancellationToken ct = default);
}