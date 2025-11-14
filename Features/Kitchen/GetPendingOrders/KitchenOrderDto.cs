namespace CampusEats.Features.Kitchen.GetPendingOrders;

using CampusEats.Features.Orders;

public record KitchenOrderDto(
    Guid Id,
    List<OrderItem> Items,
    OrderStatus Status,
    DateTime CreatedAt,
    string? SpecialInstructions
    );