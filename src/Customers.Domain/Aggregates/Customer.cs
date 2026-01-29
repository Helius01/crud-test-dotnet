using Customers.Domain.Events;
using Customers.Domain.Primitives;

namespace Customers.Domain.Aggregates;

public sealed class Customer
{
    private readonly List<DomainEvent> _uncommitted = new();
    public IReadOnlyList<DomainEvent> UncommittedEvents => _uncommitted;

    private Customer() { }

    public static Customer Create(
        Guid id,
        string firstName,
        string lastName,
        DateOnly dateOfBirth,
        string phoneNumber,
        string email,
        string bankAccountNumber)
    {
        // NOTE: validation comes later
        var customer = new Customer();
        customer.Raise(new CustomerCreatedEvent(
            id, firstName, lastName, dateOfBirth, phoneNumber, email, bankAccountNumber, DateTime.UtcNow
        ));
        return customer;
    }

    private void Raise(DomainEvent @event)
    {
        _uncommitted.Add(@event);
        // NOTE : Event sourcing comes later
    }
}
