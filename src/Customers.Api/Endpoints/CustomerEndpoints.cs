using Customers.Application.Commands.CreateCustomer;
using Customers.Application.Commands.ChangeCustomerEmail;
using Customers.Application.Queries.GetCustomerById;
using Customers.Application.ReadModel;
using MediatR;

namespace Customers.Api.Endpoints;

public static class CustomersEndpoints
{
    public static IEndpointRouteBuilder MapCustomers(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/customers");

        // POST /customers
        group.MapPost("/", async (
            CreateCustomerCommand command,
            IMediator mediator) =>
        {
            var id = await mediator.Send(command);
            return Results.Created($"/customers/{id}", new { id });
        });

        // PUT /customers/{id}/email
        group.MapPut("/{id:guid}/email", async (
            Guid id,
            ChangeEmailRequest request,
            IMediator mediator) =>
        {
            await mediator.Send(new ChangeCustomerEmailCommand(id, request.NewEmail));
            return Results.NoContent();
        });

        // GET /customers/{id}
        group.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator) =>
        {
            var customer = await mediator.Send(new GetCustomerByIdQuery(id));
            return customer is null ? Results.NotFound() : Results.Ok(customer);
        });

        return app;
    }

    public sealed record ChangeEmailRequest(string NewEmail);
}
