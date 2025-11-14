namespace CampusEats.Features.Kitchen.GetPendingOrders;

using MediatR;

public static class GetPendingOrdersEndpoint
{
    public static IEndpointRouteBuilder MapGetPendingOrders(this IEndpointRouteBuilder app)
    {
        app.MapGet("/kitchen/orders", async (IMediator mediator) =>
            {
                var query = new GetPendingOrdersQuery();
                var result = await mediator.Send(query);
                return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(result.Error);
            })
            .WithName("GetPendingOrders")
            .WithTags("Kitchen");

        return app;
    }
}