using MediatR;

namespace CampusEats.Features.Orders.GetOrderHistory;

using CampusEats.Common;

public record GetOrderHistoryQuery(string UserId) : IRequest<Result<List<OrderDto>>>;