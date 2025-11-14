namespace CampusEats.Features.Orders.CreateOrder;

using MediatR;

public static class CreateOrderEndpoint
{
    public static IEndpointRouteBuilder MapCreateOrder(this IEndpointRouteBuilder app)
    {
        app.MapPost("/orders", async (CreateOrderCommand command, IMediator mediator) =>
            {
                var result = await mediator.Send(command);
                return result.IsSuccess
                    ? Results.Created($"/orders/{result.Value}", result.Value)
                    : Results.BadRequest(result.Error);
            })
            .WithName("CreateOrder")
            .WithTags("Orders");
        
        return app;
    }
}