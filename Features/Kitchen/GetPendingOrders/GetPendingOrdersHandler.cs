using CampusEats.Features.Orders.GetOrderHistory;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace CampusEats.Features.Kitchen.GetPendingOrders;

using CampusEats.Common;
using CampusEats.Database;
using CampusEats.Features.Orders;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class GetPendingOrdersHandler : IRequestHandler<GetPendingOrdersQuery, Result<List<KitchenOrderDto>>>
{
    private readonly ApplicationDbContext _context;

    public GetPendingOrdersHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<List<KitchenOrderDto>>> Handle(GetPendingOrdersQuery request, CancellationToken ct)
    {
        var orders = await _context.Orders
            .Where(o => o.Status != OrderStatus.Completed && o.Status != OrderStatus.Cancelled)
            .OrderBy(o => o.CreatedAt)
            .Select(o => new KitchenOrderDto(
                o.Id,
                o.Items,
                o.Status,
                o.CreatedAt,
                o.SpecialInstructions
            ))
            .ToListAsync(ct);

        return Result<List<KitchenOrderDto>>.Success(orders);
    }
}