namespace CampusEats.Features.Menu.GetMenuItems;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

public static class GetMenuItemsEndpoint
{
    public static IEndpointRouteBuilder MapGetMenuItems(this IEndpointRouteBuilder app)
    {
        app.MapGet("/menu", async (
                string? category,
                [FromQuery] string[]? dietaryTags,
                IMediator mediator) =>
            {
                var tags = dietaryTags?.ToList();
                var query = new GetMenuItemsQuery(category, tags);
                var result = await mediator.Send(query);
                
                return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(result.Error);
            })
            .WithName("GetMenuItems")
            .WithTags("Menu");
        
        return app;
    }
}