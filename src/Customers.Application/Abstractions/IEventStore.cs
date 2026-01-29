using Customers.Domain.Primitives;

namespace Customers.Application.Abstractions;

public interface IEventStore
{
    Task<IReadOnlyList<DomainEvent>> LoadStreamAsync(Guid streamId, CancellationToken ct = default);

    Task AppendToStreamAsync(
        Guid streamId,
        int expectedVersion,
        IReadOnlyList<DomainEvent> events,
        CancellationToken ct = default
    );
}