using MediatR;

namespace Customers.Application.Commands.ChangeCustomerEmail;

public sealed record ChangeCustomerEmailCommand(
    Guid CustomerId,
    string NewEmail
) : IRequest;