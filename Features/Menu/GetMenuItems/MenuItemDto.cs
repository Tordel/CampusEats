namespace CampusEats.Features.Menu.GetMenuItems;

public record MenuItemDto(
    Guid Id,
    string Name,
    string Description,
    decimal Price,
    string Category,
    List<string> Allergens,
    List<string> DietaryTags,
    bool IsAvailable
    );