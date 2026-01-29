using Customers.Application.Abstractions;
using Customers.Domain.Aggregates;
using MediatR;

namespace Customers.Application.Commands.ChangeCustomerEmail;

public sealed class ChangeCustomerEmailHandler(IEventStore eventStore, IEventDispatcher eventDispatcher) : IRequestHandler<ChangeCustomerEmailCommand>
{
    private readonly IEventStore _eventStore = eventStore;
    private readonly IEventDispatcher _eventDispatcher = eventDispatcher;
    public async Task Handle(ChangeCustomerEmailCommand request, CancellationToken cancellationToken)
    {
        var history = await _eventStore.LoadStreamAsync(request.CustomerId, cancellationToken);

        if (history.Count < 1)
            throw new InvalidOperationException($"Customer '{request.CustomerId}' not found.");

        var customer = Customer.FromHistory(history);

        customer.ChangeEmail(request.NewEmail);

        var eventsToAppend = customer.UnCommittedEvents.ToList();

        await _eventStore.AppendToStreamAsync(
                   streamId: request.CustomerId,
                   expectedVersion: history.Count,
                   events: eventsToAppend,
                   ct: cancellationToken
               );

        await _eventDispatcher.DispatchEvents(eventsToAppend, cancellationToken);

        customer.ClearUncommittedEvents();
    }
}