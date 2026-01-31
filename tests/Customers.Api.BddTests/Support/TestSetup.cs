using Customers.Api.BddTests.TestHost;
using Reqnroll;

namespace Customers.Api.BddTests.Support;

[Binding]
public sealed class TestSetup
{
    private readonly ScenarioState _state;
    private readonly ApiFactory _factory;

    public TestSetup(ScenarioState state, ApiFactory factory)
    {
        _state = state;
        _factory = factory;
    }

    [BeforeScenario]
    public void BeforeScenario()
    {
        _state.Client = _factory.CreateClient();
    }
}
