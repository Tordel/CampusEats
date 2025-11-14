namespace CampusEats.Features.Orders.GetOrderHistory;

using CampusEats.Common;
using CampusEats.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class GetOrderHistoryHandler : IRequestHandler<GetOrderHistoryQuery, Result<List<OrderDto>>>
{
    private readonly ApplicationDbContext _context;
    
    public GetOrderHistoryHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<List<OrderDto>>> Handle(GetOrderHistoryQuery request, CancellationToken cancellationToken)
    {
        var orders = await _context.Orders
            .Where(o => o.UserId == request.UserId)
            .OrderByDescending(o => o.CreatedAt)
            .Select(o => new OrderDto(
                o.Id,
                o.Items,
                o.TotalAmount,
                o.Status,
                o.CreatedAt,
                o.SpecialInstructions
                ))
            .ToListAsync(cancellationToken);
        
        return Result<List<OrderDto>>.Success(orders);
    }
}