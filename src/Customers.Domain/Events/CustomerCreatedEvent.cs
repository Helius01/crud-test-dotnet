using Customers.Domain.Primitives;

namespace Customers.Domain.Events;

public sealed record CustomerCreatedEvent(
    Guid Id,
    string FirstName,
    string LastName,
    DateOnly DateOfBirth,
    string PhoneNumber,
    string Email,
    string bankAccountNumber,
    DateTime OccurredAtUTC
) : DomainEvent(OccurredAtUTC);

