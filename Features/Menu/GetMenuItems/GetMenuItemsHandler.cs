namespace CampusEats.Features.Menu.GetMenuItems;

using CampusEats.Common;
using CampusEats.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class GetMenuItemsHandler : IRequestHandler<GetMenuItemsQuery, Result<List<MenuItemDto>>>
{
    private readonly ApplicationDbContext _context;
    
    public GetMenuItemsHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<List<MenuItemDto>>> Handle(GetMenuItemsQuery request, CancellationToken cancellationToken)
    {
        var query = _context.MenuItems.Where(m => m.IsAvailable);
        
        if (!string.IsNullOrEmpty(request.Category))
            query = query.Where(m => m.Category == request.Category);

        if (request.DietaryTags?.Any() == true)
            query = query.Where(m => request.DietaryTags.All(tag => m.DietaryTags.Contains(tag)));
        
        var items = await query
            .Select(m => new MenuItemDto(
                    m.Id,
                    m.Name,
                    m.Description,
                    m.Price,
                    m.Category,
                    m.Allergens,
                    m.DietaryTags,
                    m.IsAvailable
            ))
            .ToListAsync();
        
        return Result<List<MenuItemDto>>.Success(items);
    }
}