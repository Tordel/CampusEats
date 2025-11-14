namespace CampusEats.Features.Menu.CreateMenuItem;

using MediatR;
using Microsoft.AspNetCore.Builder;

public static class CreateMenuItemEndpoint
{
    public static IEndpointRouteBuilder MapCreateMenuItem(this IEndpointRouteBuilder app)
    {
        app.MapPost("/menu", async (CreateMenuItemCommand command, IMediator mediator) =>
            {
                var result = await mediator.Send(command);
                return result.IsSuccess
                    ? Results.Created($"/menu/{result.Value}", result.Value)
                    : Results.BadRequest(result.Error);
            })
            .WithName("CreateMenuItem")
            .WithTags("Menu");
        
        return app;
    }
}