namespace ECommerce.Application.Orders;

public record OrderDetailsDto(
    int Id,
    DateTime CreatedAt,
    decimal Total,
    List<OrderItemDto> Items);
