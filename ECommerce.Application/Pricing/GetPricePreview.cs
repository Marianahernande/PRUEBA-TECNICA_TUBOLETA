using MediatR;

namespace ECommerce.Application.Pricing;

public record GetPricePreviewQuery(int ProductId, DateTime Date) : IRequest<decimal>;

public class GetPricePreviewHandler(IPricingService pricing) : IRequestHandler<GetPricePreviewQuery, decimal>
{
    public Task<decimal> Handle(GetPricePreviewQuery req, CancellationToken ct)
        => pricing.PreviewAsync(req.ProductId, req.Date, ct);
}
