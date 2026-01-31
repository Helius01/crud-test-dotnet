namespace Customers.Api.BddTests.Support;

public sealed class ScenarioState
{
    public HttpClient Client { get; set; } = default!;
    public HttpResponseMessage? LastResponse { get; set; }
    public Guid CustomerId { get; set; }
}
