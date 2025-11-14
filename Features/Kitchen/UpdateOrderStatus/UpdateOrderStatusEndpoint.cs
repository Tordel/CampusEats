namespace CampusEats.Features.Kitchen.UpdateOrderStatus;

using CampusEats.Features.Orders;
using MediatR;

public static class UpdateOrderStatusEndpoint
{
    public static IEndpointRouteBuilder MapUpdateOrderStatus(this IEndpointRouteBuilder app)
    {
        app.MapPatch("/kitchen/orders/{orderId}/status", async (
                Guid orderId,
                OrderStatus status,
                IMediator mediator) =>
            {
                var command = new UpdateOrderStatusCommand(orderId, status);
                var result = await mediator.Send(command);
                return result.IsSuccess ? Results.Ok() : Results.BadRequest(result.Error);
            })
            .WithName("UpdateOrderStatus")
            .WithTags("Kitchen");

        return app;
    }
}