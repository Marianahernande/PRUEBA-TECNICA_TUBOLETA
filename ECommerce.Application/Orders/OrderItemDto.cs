namespace ECommerce.Application.Orders;

public record OrderItemDto(int ProductId, string Product, int Quantity, decimal UnitPrice, decimal Subtotal);
