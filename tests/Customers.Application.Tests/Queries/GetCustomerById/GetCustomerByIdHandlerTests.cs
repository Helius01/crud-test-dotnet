using Customers.Application.Queries.GetCustomerById;
using Customers.Application.ReadModel;
using FluentAssertions;

namespace Customers.Application.Tests.Queries.GetCustomerById;

public class GetCustomerByIdHandlerTests
{
    private sealed class FakeStore : ICustomerReadStore
    {
        private readonly Dictionary<Guid, CustomerReadDto> _db = new();

        public Task UpsertAsync(CustomerReadDto customer, CancellationToken ct = default)
        {
            _db[customer.Id] = customer;
            return Task.CompletedTask;
        }

        public Task<CustomerReadDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            _db.TryGetValue(id, out var v);
            return Task.FromResult(v);
        }

        public Task<IReadOnlyList<CustomerReadDto>> ListAsync(CancellationToken ct = default)
            => Task.FromResult<IReadOnlyList<CustomerReadDto>>(_db.Values.ToList());
    }

    [Fact]
    public async Task Handle_should_return_customer_when_exists()
    {
        var store = new FakeStore();
        var id = Guid.NewGuid();

        await store.UpsertAsync(new CustomerReadDto(
            id, "A", "B", new DateOnly(2000, 1, 1), "123", "a@b.com", "X"
        ));

        var handler = new GetCustomerByIdQueryHandler(store);

        var result = await handler.Handle(new GetCustomerByIdQuery(id), CancellationToken.None);

        result.Should().NotBeNull();
        result!.Email.Should().Be("a@b.com");
    }
}
