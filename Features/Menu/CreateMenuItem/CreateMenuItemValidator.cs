namespace CampusEats.Features.Menu.CreateMenuItem;

using FluentValidation;

public class CreateMenuItemValidator : AbstractValidator<CreateMenuItemCommand>
{
    public CreateMenuItemValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Description).MaximumLength(1000);
        RuleFor(x => x.Price).GreaterThan(0).LessThan(1000);
        RuleFor(x => x.Category).NotEmpty().MaximumLength(50);
    }
}