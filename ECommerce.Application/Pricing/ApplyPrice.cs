 using MediatR;

namespace ECommerce.Application.Pricing;

public record ApplyPriceCommand(int ProductId, DateTime Date) : IRequest<decimal>;

public class ApplyPriceHandler(IPricingService pricing) : IRequestHandler<ApplyPriceCommand, decimal>
{
    public Task<decimal> Handle(ApplyPriceCommand req, CancellationToken ct)
        => pricing.ApplyAsync(req.ProductId, req.Date, ct);
}
