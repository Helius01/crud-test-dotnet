using Customers.Application.Abstractions;
using Customers.Application.Commands.ChangeCustomerEmail;
using Customers.Domain.Events;
using Customers.Domain.Primitives;
using FluentAssertions;

namespace Customers.Application.Tests.Commands.ChangeCustomerEmail;

public class ChangeCustomerEmailHandlerTests
{
    private sealed class NoOpEventDispatcher : IEventDispatcher
    {
        public Task DispatchEvents(IReadOnlyList<DomainEvent> events, CancellationToken ct = default) => Task.CompletedTask;
    }


    private sealed class FakeEventStore : IEventStore
    {
        private readonly IReadOnlyList<DomainEvent> _history;

        public Guid? AppendedStreamId { get; private set; }
        public int? AppendedExpectedVersion { get; private set; }
        public IReadOnlyList<DomainEvent>? AppendedEvents { get; private set; }

        public FakeEventStore(IReadOnlyList<DomainEvent> history)
        {
            _history = history;
        }

        public Task<IReadOnlyList<DomainEvent>> LoadStreamAsync(
            Guid streamId,
            CancellationToken ct = default)
            => Task.FromResult(_history);

        public Task AppendToStreamAsync(
            Guid streamId,
            int expectedVersion,
            IReadOnlyList<DomainEvent> events,
            CancellationToken ct = default)
        {
            AppendedStreamId = streamId;
            AppendedExpectedVersion = expectedVersion;
            AppendedEvents = events.ToList();
            return Task.CompletedTask;
        }
    }

    [Fact]
    public async Task Handle_Should_Throw_When_Customer_Not_Found()
    {
        var id = Guid.NewGuid();

        IReadOnlyList<DomainEvent> history = Array.Empty<DomainEvent>();

        var store = new FakeEventStore(history);
        var dispatcher = new NoOpEventDispatcher();

        var handler = new ChangeCustomerEmailHandler(store, dispatcher);

        var act = async () => await handler.Handle(
            new ChangeCustomerEmailCommand(id, "new@example.com"),
            CancellationToken.None
        );

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage($"Customer '{id}' not found.");

        store.AppendedEvents.Should().BeNull();
    }
    [Fact]
    public async Task Handle_Should_Append_Email_Changed_Event_Using_Expected_Version()
    {
        var id = Guid.NewGuid();

        IReadOnlyList<DomainEvent> history =
        [
            new CustomerCreatedEvent(
                Id: id,
                FirstName: "Mohammad",
                LastName: "Ahmadi",
                DateOfBirth: new DateOnly(2000, 1, 1),
                PhoneNumber: "+994501234567",
                Email: "old@example.com",
                BankAccountNumber: "X",
                OccurredAtUTC: DateTime.UtcNow
            )
        ];

        var store = new FakeEventStore(history);
        var dispatcher = new NoOpEventDispatcher();
        var handler = new ChangeCustomerEmailHandler(store, dispatcher);

        var cmd = new ChangeCustomerEmailCommand(id, "new@example.com");

        await handler.Handle(cmd, CancellationToken.None);

        store.AppendedExpectedVersion.Should().Be(1);
        store.AppendedEvents.Should().ContainSingle()
            .Which.Should().BeOfType<CustomerEmailChangedEvent>();
    }
}
