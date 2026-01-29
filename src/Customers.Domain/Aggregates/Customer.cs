using Customers.Domain.Events;
using Customers.Domain.Primitives;
using Customers.Domain.ValueObjects;

namespace Customers.Domain.Aggregates;

public sealed class Customer : AggregateRoot
{
    public Guid Id { get; private set; }
    public string FirstName { get; private set; } = default!;
    public string LastName { get; private set; } = default!;
    public DateOnly DateOfBirth { get; private set; }
    public string PhoneNumber { get; private set; } = default!;
    public Email Email { get; private set; } = default!;
    public string BankAccountNumber { get; private set; } = default!;

    private Customer() { }

    public static Customer Create(
        Guid id,
        string firstName,
        string lastName,
        DateOnly dateOfBirth,
        string phoneNumber,
        Email email,
        string bankAccountNumber)
    {
        // NOTE: validation comes later
        var customer = new Customer();
        var normalizedEmail = email.Value;

        customer.Raise(new CustomerCreatedEvent(
            id, firstName, lastName, dateOfBirth, phoneNumber, normalizedEmail, bankAccountNumber, DateTime.UtcNow
        ));

        return customer;
    }

    public void ChangeEmail(string newEmail)
    {
        var normalized = Email.Create(newEmail).Value;

        if (Email.Value == normalized)
            throw new InvalidOperationException("Email is already set to this value.");

        Raise(new CustomerEmailChangedEvent(
            Id: Id,
            NewEmail: normalized,
            OccurredAtUTC: DateTime.UtcNow
        ));
    }


    public static Customer FromHistory(IEnumerable<DomainEvent> history)
    {
        var customer = new Customer();
        customer.LoadFromHistory(history);
        customer.ClearUncommittedEvents();
        return customer;
    }

    protected override void Apply(DomainEvent @event)
    {
        switch (@event)
        {
            case CustomerCreatedEvent e:
                Id = e.Id;
                FirstName = e.FirstName;
                LastName = e.LastName;
                DateOfBirth = e.DateOfBirth;
                PhoneNumber = e.PhoneNumber;
                Email = Email.Create(e.Email);
                BankAccountNumber = e.BankAccountNumber;
                break;
            case CustomerEmailChangedEvent e:
                Email = Email.Create(e.NewEmail);
                break;
            default:
                throw new InvalidOperationException(
                    $"Unsupported event type '{@event.GetType().Name}' in {nameof(Customer)}");
        }
    }
}
