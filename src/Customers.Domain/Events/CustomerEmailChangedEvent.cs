using Customers.Domain.Primitives;

namespace Customers.Domain.Events;

public sealed record CustomerEmailChangedEvent(
    Guid Id,
    string NewEmail,
    DateTime OccurredAtUTC
) : DomainEvent(OccurredAtUTC);