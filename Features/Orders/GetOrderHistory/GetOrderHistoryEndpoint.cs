using Microsoft.AspNetCore.SignalR;

namespace CampusEats.Features.Orders.GetOrderHistory;

using MediatR;

public static class GetOrderHistoryEndpoint
{
    public static IEndpointRouteBuilder MapGetOrderHistory(this IEndpointRouteBuilder app)
    {
        app.MapGet("/orders/user/{userId}", async (string userId, IMediator mediator) =>
            {
                var query = new GetOrderHistoryQuery(userId);
                var result = await mediator.Send(query);
                return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(result.Error);
            })
            .WithName("GetOrderHistory")
            .WithTags("Orders");

        return app;
    }
}