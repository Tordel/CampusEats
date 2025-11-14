namespace CampusEats.Features.Orders.CreateOrder;

using CampusEats.Common;
using CampusEats.Database;
using CampusEats.Features.Orders;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class CreateOrderHandler : IRequestHandler<CreateOrderCommand, Result<Guid>>
{
    private readonly ApplicationDbContext _context;
    
    public CreateOrderHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<Guid>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var menuItemIds = request.Items.Select(i => i.MenuItemId).ToList();
        var menuItems = await _context.MenuItems
            .Where(m => menuItemIds.Contains(m.Id) && m.IsAvailable)
            .ToListAsync(cancellationToken);

        if (menuItems.Count != menuItemIds.Distinct().Count())
            return Result<Guid>.Failure("Some menu items are not available");

        var orderItems = request.Items.Select(dto =>
        {
            var menuItem = menuItems.First(m => m.Id == dto.MenuItemId);
            return new OrderItem
            {
                Id = Guid.NewGuid(),
                MenuItemId = menuItem.Id,
                MenuItemName = menuItem.Name,
                Quantity = dto.Quantity,
                Price = menuItem.Price
            };
        }).ToList();

        var order = new Order
        {
            Id = Guid.NewGuid(),
            UserId = request.UserId,
            Items = orderItems,
            TotalAmount = orderItems.Sum(i => i.Price * i.Quantity),
            Status = OrderStatus.Pending,
            SpecialInstructions = request.SpecialInstructions,
            CreatedAt = DateTime.UtcNow
        };

        _context.Orders.Add(order);
        await _context.SaveChangesAsync(cancellationToken);
        
        return Result<Guid>.Success(order.Id);
    }
}