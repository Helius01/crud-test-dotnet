using System.Runtime.CompilerServices;

namespace Customers.Domain.Primitives;

public abstract class AggregateRoot
{
    private readonly List<DomainEvent> _uncommitted = new();
    public int Version { get; protected set; } = 0;
    public IReadOnlyList<DomainEvent> UnCommittedEvents => _uncommitted;

    protected void Raise(DomainEvent @event)
    {
        Apply(@event);
        _uncommitted.Add(@event);
    }
    protected abstract void Apply(DomainEvent @event);

    public void ClearUncommittedEvents() => _uncommitted.Clear();

    protected void LoadFromHistory(IEnumerable<DomainEvent> history)
    {
        foreach (var e in history)
        {
            Apply(e);
            Version++;
        }
    }
}