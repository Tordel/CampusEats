namespace CampusEats.Features.Kitchen.GetPendingOrders;

using CampusEats.Common;
using MediatR;

public record GetPendingOrdersQuery : IRequest<Result<List<KitchenOrderDto>>>;