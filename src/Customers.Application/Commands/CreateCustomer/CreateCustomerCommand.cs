using MediatR;

namespace Customers.Application.Commands.CreateCustomer;

public sealed record CreateCustomerCommand(
    Guid Id,
    string FirstName,
    string LastName,
    DateOnly DateOfBirth,
    string PhoneNumber,
    string Email,
    string BankAccountNumber
) : IRequest<Guid>;