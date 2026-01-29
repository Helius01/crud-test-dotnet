
using FluentAssertions;
using Customers.Domain.Aggregates;
using Customers.Domain.Events;
using Customers.Domain.Primitives;

namespace Customers.Domain.Tests.Aggregates;

public class CustomerChangeEmailTests
{
    [Fact]
    public void ChangeEmail_Should_Emit_CustomerEmailChanged()
    {
        var id = Guid.NewGuid();

        DomainEvent[] history =
        {
            new CustomerCreatedEvent(
                Id: id,
                FirstName: "Mohammad",
                LastName: "Abedi",
                DateOfBirth: new DateOnly(2000, 1, 1),
                PhoneNumber: "+994501234567",
                Email: "old@test.com",
                BankAccountNumber: "X123",
                OccurredAtUTC: DateTime.UtcNow
            )
        };

        var customer = Customer.FromHistory(history);

        customer.ChangeEmail("new@test.com");

        customer.UnCommittedEvents.Should().ContainSingle();
        customer.UnCommittedEvents[0].Should().BeOfType<CustomerEmailChangedEvent>()
            .Which.NewEmail.Should().Be("new@test.com");

        customer.Email.Value.Should().Be("new@test.com");
    }

    [Fact]
    public void ChangeEmail_Should_Throw_When_New_Email_Is_Same_As_Current()
    {
        var id = Guid.NewGuid();

        DomainEvent[] history =
        {
            new CustomerCreatedEvent(
                Id: id,
                FirstName: "Mohammad",
                LastName: "Abedi",
                DateOfBirth: new DateOnly(2000, 1, 1),
                PhoneNumber: "+994501234567",
                Email: "same@test.com",
                BankAccountNumber: "X123",
                OccurredAtUTC: DateTime.UtcNow
            )
        };

        var customer = Customer.FromHistory(history);

        var act = () => customer.ChangeEmail("same@test.com");

        act.Should().Throw<InvalidOperationException>();
        customer.UnCommittedEvents.Should().BeEmpty();
    }

    [Fact]
    public void ChangeEmail_Should_Throw_For_Invalid_Email()
    {
        var id = Guid.NewGuid();

        DomainEvent[] history =
        {
            new CustomerCreatedEvent(
                Id: id,
                FirstName: "Mohammad",
                LastName: "Abedi",
                DateOfBirth: new DateOnly(2000, 1, 1),
                PhoneNumber: "+994501234567",
                Email: "old@test.com",
                BankAccountNumber: "X123",
                OccurredAtUTC: DateTime.UtcNow
            )
        };

        var customer = Customer.FromHistory(history);

        var act = () => customer.ChangeEmail("not-an-email");

        act.Should().Throw<ArgumentException>();
        customer.UnCommittedEvents.Should().BeEmpty();
    }
}
