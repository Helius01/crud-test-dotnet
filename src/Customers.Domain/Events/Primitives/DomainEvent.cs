namespace Customers.Domain.Primitives;

public abstract record DomainEvent(DateTime OccurredAtUTC);