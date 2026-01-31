using Customers.Api.BddTests.Support;
using Microsoft.Extensions.DependencyInjection;
using Reqnroll;
using Reqnroll.Bindings;


namespace Customers.Api.BddTests;

[Binding]
public sealed class ReqnrollHooks
{
    public static IServiceCollection CreateServices()
    {
        var services = new ServiceCollection();
        services.AddBddServices();
        return services;
    }
}
