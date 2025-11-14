namespace CampusEats.Features.Menu.CreateMenuItem;

using CampusEats.Database;
using CampusEats.Common;
using MediatR;

public class CreateMenuItemHandler : IRequestHandler<CreateMenuItemCommand, Result<Guid>>
{
    private readonly ApplicationDbContext _context;

    public CreateMenuItemHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<Guid>> Handle(CreateMenuItemCommand request, CancellationToken cancellationToken)
    {
        var menuItem = new MenuItem
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            Category = request.Category,
            Allergens = request.Allergens ?? new(),
            DietaryTags = request.DietaryTags ?? new(),
            IsAvailable = true,
            CreatedAt = DateTime.UtcNow
        };

        _context.MenuItems.Add(menuItem);
        await _context.SaveChangesAsync(cancellationToken);
        
        return Result<Guid>.Success(menuItem.Id);
    }
}