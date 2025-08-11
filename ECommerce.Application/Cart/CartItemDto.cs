namespace ECommerce.Application.Cart;
public record CartItemDto(int Id, int ProductId, string Product, decimal Price, int Quantity, decimal Subtotal);
