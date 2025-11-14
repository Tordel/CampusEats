using CampusEats.Database;
using CampusEats.Features.Kitchen.GetPendingOrders;
using CampusEats.Features.Kitchen.UpdateOrderStatus;
using CampusEats.Features.Menu.CreateMenuItem;
using CampusEats.Features.Menu.GetMenuItems;
using CampusEats.Features.Orders.CreateOrder;
using CampusEats.Features.Orders.GetOrderHistory;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Swagger;

var builder = WebApplication.CreateBuilder(args);

// Database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// MediatR
builder.Services.AddMediatR(cfg => 
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

// Validation
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

// Swagger - ADD THESE!
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Swagger UI - ADD THIS!
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Endpoints
app.MapCreateMenuItem();
app.MapGetMenuItems();
app.MapCreateOrder();
app.MapGetOrderHistory();
app.MapGetPendingOrders();
app.MapUpdateOrderStatus();

app.Run();

// Validation behavior (keep this at the bottom)
public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!_validators.Any())
            return await next();

        var context = new ValidationContext<TRequest>(request);
        var validationResults = await Task.WhenAll(
            _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

        var failures = validationResults
            .SelectMany(r => r.Errors)
            .Where(f => f != null)
            .ToList();

        if (failures.Count != 0)
        {
            var errors = string.Join("; ", failures.Select(f => f.ErrorMessage));
            
            if (typeof(TResponse).IsGenericType && 
                typeof(TResponse).GetGenericTypeDefinition() == typeof(CampusEats.Common.Result<>))
            {
                var resultType = typeof(TResponse).GetGenericArguments()[0];
                var failureMethod = typeof(CampusEats.Common.Result<>)
                    .MakeGenericType(resultType)
                    .GetMethod("Failure");
                return (TResponse)failureMethod!.Invoke(null, new object[] { errors })!;
            }
            
            return (TResponse)(object)CampusEats.Common.Result.Failure(errors);
        }

        return await next();
    }
}