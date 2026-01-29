using Customers.Domain.Primitives;
using Customers.Domain.ValueObjects;

namespace Customers.Domain.Events;

public sealed record CustomerCreatedEvent(
    Guid Id,
    string FirstName,
    string LastName,
    DateOnly DateOfBirth,
    string PhoneNumber,
    string Email,
    string BankAccountNumber,
    DateTime OccurredAtUTC
) : DomainEvent(OccurredAtUTC);

