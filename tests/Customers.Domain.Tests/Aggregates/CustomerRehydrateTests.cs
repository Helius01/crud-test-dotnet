using System.Reflection.Metadata;
using Customers.Domain.Aggregates;
using Customers.Domain.Events;
using Customers.Domain.Primitives;
using Customers.Domain.ValueObjects;
using FluentAssertions;

namespace Customers.Domain.Tests.Aggregates;

public class CustomerRehydrateTests
{
    [Fact]
    public void FromHistory_Should_Restore_State_And_Have_No_UnCommitted_Events()
    {
        var id = Guid.NewGuid();

        DomainEvent[] history =
        {
            new CustomerCreatedEvent(id,"Mohammad","Abedi",new DateOnly(2000,1,1),"09101111111","test@test.com","123",DateTime.UtcNow)
        };

        var customer = Customer.FromHistory(history);
        customer.Id.Should().Be(id);
        customer.Email.Value.Should().Be("test@test.com");
        customer.UnCommittedEvents.Should().BeEmpty();
        customer.Version.Should().Be(1);
    }
}