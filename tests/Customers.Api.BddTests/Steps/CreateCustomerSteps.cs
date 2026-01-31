using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Customers.Api.BddTests.Support;
using Customers.Application.Commands.CreateCustomer;
using Customers.Application.ReadModel;
using FluentAssertions;
using Reqnroll;

namespace Customers.Api.BddTests.Steps;

[Binding]
public sealed class CreateCustomerSteps
{
    private readonly ScenarioState _state;

    public CreateCustomerSteps(ScenarioState state) => _state = state;

    [Given("I have a new customer id")]
    public void GivenIHaveANewCustomerId()
    {
        _state.CustomerId = Guid.NewGuid();
    }

    [When("I create a customer with email {string}")]
    public async Task WhenICreateACustomerWithEmail(string email)
    {
        var cmd = new CreateCustomerCommand(
            Id: _state.CustomerId,
            FirstName: "Mohammad",
            LastName: "Abedi",
            DateOfBirth: new DateOnly(2000, 1, 1),
            PhoneNumber: "+994501234567",
            Email: email,
            BankAccountNumber: "X"
        );

        _state.LastResponse = await _state.Client.PostAsJsonAsync("/customers", cmd);
    }

    [Then("the response status should be 201")]
    public void ThenTheResponseStatusShouldBe201()
    {
        _state.LastResponse!.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Then("fetching the customer should return email {string}")]
    public async Task ThenFetchingTheCustomerShouldReturnEmail(string email)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var customer = await _state.Client.GetFromJsonAsync<CustomerReadDto>(
            $"/customers/{_state.CustomerId}",
            options
        );

        customer.Should().NotBeNull();
        customer!.Email.Should().Be(email);
    }
}
