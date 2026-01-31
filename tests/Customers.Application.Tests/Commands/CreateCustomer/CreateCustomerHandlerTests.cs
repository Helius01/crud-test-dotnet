using Customers.Application.Abstractions;
using Customers.Application.Commands.CreateCustomer;
using Customers.Domain.Primitives;
using FluentAssertions;

namespace Customers.Application.Tests.Commands.CreateCustomer;

public class CreateCustomerHandlerTests
{
    private sealed class NoOpEventDispatcher : IEventDispatcher
    {
        public Task DispatchEvents(IReadOnlyList<DomainEvent> events, CancellationToken ct = default)
        {
            return Task.CompletedTask;
        }
    }

    private sealed class FakeEventStore : IEventStore
    {
        public Guid? StreamId { get; private set; }
        public int? ExpectedVersion { get; private set; }
        public IReadOnlyList<DomainEvent>? Appended { get; private set; }

        public Task<IReadOnlyList<DomainEvent>> LoadStreamAsync(Guid streamId, CancellationToken ct = default)
            => Task.FromResult<IReadOnlyList<DomainEvent>>(Array.Empty<DomainEvent>());

        public Task AppendToStreamAsync(Guid streamId, int expectedVersion, IReadOnlyList<DomainEvent> events, CancellationToken ct = default)
        {
            StreamId = streamId;
            ExpectedVersion = expectedVersion;
            Appended = events;
            return Task.CompletedTask;
        }
    }

    [Fact]
    public async Task Handle_Should_Append_Customer_Created_Event()
    {
        var store = new FakeEventStore();
        var dispatcher = new NoOpEventDispatcher();

        var handler = new CreateCustomerHandler(store, dispatcher);

        var id = Guid.NewGuid();
        var cmd = new CreateCustomerCommand(
            Id: id,
            FirstName: "Mohammad",
            LastName: "Abedi",
            DateOfBirth: new DateOnly(2000, 1, 1),
            PhoneNumber: "+994501234567",
            Email: "test@test.com",
            BankAccountNumber: "X"
        );

        var result = await handler.Handle(cmd, CancellationToken.None);

        result.Should().Be(id);
        store.StreamId.Should().Be(id);
        store.ExpectedVersion.Should().Be(0);
        store.Appended.Should().NotBeNull();
        store.Appended!.Count.Should().Be(1);
        store.Appended![0].GetType().Name.Should().Be("CustomerCreatedEvent");
    }
}
