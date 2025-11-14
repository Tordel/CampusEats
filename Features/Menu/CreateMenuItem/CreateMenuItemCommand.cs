using CampusEats.Common;
using MediatR;

namespace CampusEats.Features.Menu;

public record CreateMenuItemCommand(
    string Name,
    string Description,
    decimal Price,
    string Category,
    List<string>? Allergens,
    List<string>? DietaryTags
    ) : IRequest<Result<Guid>>;