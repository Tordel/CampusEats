namespace CampusEats.Features.Orders.GetOrderHistory;

using CampusEats.Features.Orders;

public record OrderDto(
    Guid Id,
    List<OrderItem> Items,
    decimal TotalAmount,
    OrderStatus Status,
    DateTime CreatedAt,
    string? SpecialInstructions
    );