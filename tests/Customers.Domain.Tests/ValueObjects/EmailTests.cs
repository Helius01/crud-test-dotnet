using Customers.Domain.ValueObjects;
using FluentAssertions;
namespace Customers.Domain.Tests.ValueObjects;

public class EmailTests
{
    [Theory]
    [InlineData("")]
    [InlineData("    ")]
    [InlineData("not-an-email")]
    [InlineData("a@")]
    [InlineData("a@.com")]
    public void Create_Should_Throw_For_Invalid_Emails(string invalid)
    {
        var act = () => Email.Create(invalid);

        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Email_Should_Normalize_After_Create()
    {
        var email = Email.Create("   Test@Test.com");

        email.Value.Should().Be("test@test.com");
    }
}