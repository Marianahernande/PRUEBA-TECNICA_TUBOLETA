using ECommerce.Application.Pricing;
using ECommerce.Application.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Pricing;
public class PricingService(IAppDbContext db) : IPricingService
{
    // Pricing ultra simple: 5% si hay poco inventario (si no tienes inventario, ignora la parte de stock)
    public decimal Preview(int productId, DateTime asOfUtc)
    {
        var price = db.Products.AsNoTracking().Where(p => p.Id == productId).Select(p => p.Price).FirstOrDefault();
        if (price <= 0) return 0m;
        // lógica mínima estable
        return Math.Round(price * 1.00m, 2); // deja el mismo precio por ahora (o aplica un % si quieres)
    }

    public decimal Apply(int productId, DateTime asOfUtc) => Preview(productId, asOfUtc);
}
