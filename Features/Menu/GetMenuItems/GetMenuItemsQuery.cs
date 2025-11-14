namespace CampusEats.Features.Menu.GetMenuItems;

using CampusEats.Common;
using MediatR;

public record GetMenuItemsQuery(
    string? Category,
    List<string>? DietaryTags
    ) : IRequest<Result<List<MenuItemDto>>>;