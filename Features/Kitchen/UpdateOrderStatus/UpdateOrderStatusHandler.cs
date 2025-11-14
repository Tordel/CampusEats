using System.Data;

namespace CampusEats.Features.Kitchen.UpdateOrderStatus;

using CampusEats.Common;
using CampusEats.Database;
using CampusEats.Features.Orders;
using MediatR;

public class UpdateOrderStatusHandler : IRequestHandler<UpdateOrderStatusCommand, Result>
{
    private readonly ApplicationDbContext _context;
    
    public  UpdateOrderStatusHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(UpdateOrderStatusCommand request, CancellationToken cancellationToken)
    {
        var order = await _context.Orders.FindAsync(new object[] { request.OrderId }, cancellationToken);

        if (order == null)
            return Result.Failure("Order not found");

        if (!IsValidStatusTransition(order.Status, request.NewStatus))
            return Result.Failure($"Cannot transition from {order.Status} to {request.NewStatus}");

        order.Status = request.NewStatus;

        if (request.NewStatus == OrderStatus.Completed)
            order.CompletedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }

    private static bool IsValidStatusTransition(OrderStatus current, OrderStatus next)
    {
        return (current, next) switch
        {
            (OrderStatus.Pending, OrderStatus.Confirmed) => true,
            (OrderStatus.Confirmed, OrderStatus.Preparing) => true,
            (OrderStatus.Preparing, OrderStatus.Ready) => true,
            (OrderStatus.Ready, OrderStatus.Completed) => true,
            (OrderStatus.Pending or OrderStatus.Confirmed, OrderStatus.Cancelled) => true,
            _ => false
        };
    }
}