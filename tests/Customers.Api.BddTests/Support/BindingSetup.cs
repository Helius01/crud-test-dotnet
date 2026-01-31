using Customers.Api.BddTests.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Reqnroll;

namespace Customers.Api.BddTests.Support;

[Binding]
public sealed class BindingsSetup
{
    [BeforeScenario]
    public void BeforeScenario(ScenarioContext ctx)
    {

    }
}

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBddServices(this IServiceCollection services)
    {
        services.AddSingleton<ApiFactory>();
        services.AddScoped(sp => sp.GetRequiredService<ApiFactory>().CreateClient());
        services.AddScoped<ScenarioState>();
        return services;
    }
}
