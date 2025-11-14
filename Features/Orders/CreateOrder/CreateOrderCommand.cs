namespace CampusEats.Features.Orders.CreateOrder;

using CampusEats.Common;
using MediatR;

public record CreateOrderCommand(
    string UserId,
    List<OrderItemRequest> Items,
    string? SpecialInstructions
    ) : IRequest<Result<Guid>>;