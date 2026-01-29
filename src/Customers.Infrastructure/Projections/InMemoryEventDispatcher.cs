using Customers.Application.Abstractions;
using Customers.Domain.Primitives;

namespace Customers.Infrastructure.Projections;

public sealed class InMemoryEventDispatcher : IEventDispatcher
{
    private readonly CustomerProjection _customerProjection;

    public InMemoryEventDispatcher(CustomerProjection customerProjection)
        => _customerProjection = customerProjection;

    public Task DispatchEvents(IReadOnlyList<DomainEvent> events, CancellationToken ct = default)
        => _customerProjection.ProjectAsync(events, ct);
}
