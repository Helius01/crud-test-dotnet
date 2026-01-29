using Customers.Domain.Aggregates;
using Customers.Domain.Events;
using Customers.Domain.ValueObjects;
using FluentAssertions;

namespace Customers.Domain.Tests.Aggregates;

public class CustomerCreateTests
{
    [Fact]
    public void Create_should_emit_CustomerCreated_with_correct_data()
    {
        var id = Guid.NewGuid();

        var customer = Customer.Create(
            id: id,
            firstName: "Mohammad",
            lastName: "Abedi",
            dateOfBirth: new DateOnly(2001, 05, 15),
            phoneNumber: "091011111111",
            email: Email.Create("abedi@mail.com"),
            bankAccountNumber: "AZ00TEST1234567890"
        );

        customer.UncommittedEvents.Should().ContainSingle();

        var ev = customer.UncommittedEvents.Single().Should().BeOfType<CustomerCreatedEvent>().Subject;
        ev.Id.Should().Be(id);
        ev.FirstName.Should().Be("Mohammad");
        ev.LastName.Should().Be("Abedi");
        ev.Email.Should().Be("abedi@mail.com");
    }
}
