using System.Collections.Concurrent;
using Customers.Application.ReadModel;

namespace Customers.Infrastructure.ReadModel;

public class InMemoryCustomerReadStore : ICustomerReadStore
{
    private readonly ConcurrentDictionary<Guid, CustomerReadDto> _db = new();

    public Task<CustomerReadDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        _db.TryGetValue(id, out var customer);

        return Task.FromResult(customer);
    }

    public Task<IReadOnlyList<CustomerReadDto>> ListAsync(CancellationToken ct = default)
    {
        var customers = _db.Values.OrderBy(x => x.LastName).ThenBy(x => x.FirstName).ToList();

        return Task.FromResult<IReadOnlyList<CustomerReadDto>>(customers);
    }

    public Task UpsertAsync(CustomerReadDto customer, CancellationToken cancellationToken = default)
    {
        _db[customer.Id] = customer;
        return Task.CompletedTask;
    }
}