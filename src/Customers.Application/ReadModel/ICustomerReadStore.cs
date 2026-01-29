using Customers.Domain.Aggregates;

namespace Customers.Application.ReadModel;

public interface ICustomerReadStore
{
    Task UpsertAsync(CustomerReadDto customer, CancellationToken cancellationToken = default);
    Task<CustomerReadDto?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<IReadOnlyList<CustomerReadDto>> ListAsync(CancellationToken ct = default);
}