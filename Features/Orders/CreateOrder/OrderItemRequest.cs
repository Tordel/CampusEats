namespace CampusEats.Features.Orders.CreateOrder;

public record OrderItemRequest(Guid MenuItemId, int Quantity);