using CampusEats.Features.Orders.GetOrderHistory;

namespace CampusEats.Features.Kitchen.UpdateOrderStatus;

using CampusEats.Common;
using CampusEats.Features.Orders;
using MediatR;

public record UpdateOrderStatusCommand(
    Guid OrderId,
    OrderStatus NewStatus
    ) : IRequest<Result>;