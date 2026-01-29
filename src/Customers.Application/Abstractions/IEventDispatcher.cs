using Customers.Domain.Primitives;

namespace Customers.Application.Abstractions;

public interface IEventDispatcher
{
    Task DispatchEvents(IReadOnlyList<DomainEvent> events, CancellationToken ct = default);
}