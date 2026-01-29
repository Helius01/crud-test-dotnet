using Customers.Domain.Aggregates;
using Customers.Domain.ValueObjects;
using FluentAssertions;

namespace Customers.Domain.Tests.Aggregates;

public class CustomerEmailTests
{
    [Fact]
    public void Create_Customer_Should_Throw_For_Invalid_Email()
    {
        var act = () => Customer.Create(
            id: Guid.NewGuid(),
            firstName: "A",
            lastName: "B",
            dateOfBirth: new DateOnly(2000, 1, 1),
            phoneNumber: "123",
            email: Email.Create("invalid_email"),
            bankAccountNumber: "x"
        );

        act.Should().Throw<ArgumentException>();
    }
}