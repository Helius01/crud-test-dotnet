using System.Collections.Concurrent;
using Customers.Application.Abstractions;
using Customers.Domain.Primitives;

namespace Customers.Infrastructure.EventStore;

public class InMemoryEventStore : IEventStore
{
    private readonly ConcurrentDictionary<Guid, List<DomainEvent>> _streams = new();

    public Task AppendToStreamAsync(Guid streamId, int expectedVersion, IReadOnlyList<DomainEvent> events, CancellationToken ct = default)
    {
        if (events is null || events.Count == 0)
            return Task.CompletedTask;

        var stream = _streams.GetOrAdd(streamId, _ => new List<DomainEvent>());

        lock (stream)
        {
            var actualVersion = stream.Count;

            if (actualVersion != expectedVersion)
                throw new InvalidOperationException(
                    $"Concurrency conflict for stream {streamId}. Expected {expectedVersion} but was {actualVersion}.");

            stream.AddRange(events);
        }

        return Task.CompletedTask;
    }

    public Task<IReadOnlyList<DomainEvent>> LoadStreamAsync(Guid streamId, CancellationToken ct = default)
    {
        _streams.TryGetValue(streamId, out var events);

        IReadOnlyList<DomainEvent> result = events?.ToList() ?? new List<DomainEvent>();

        return Task.FromResult(result);
    }
}