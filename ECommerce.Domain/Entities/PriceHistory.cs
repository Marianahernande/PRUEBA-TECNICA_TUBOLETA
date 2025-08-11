namespace ECommerce.Domain.Entities;

public class PriceHistory
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public decimal OldPrice { get; set; }
    public decimal NewPrice { get; set; }
    public DateTime AppliedAt { get; set; }

    public Product? Product { get; set; }
}
