using Customers.Application.ReadModel;
using Customers.Domain.Events;
using Customers.Domain.Primitives;

namespace Customers.Infrastructure.Projections;

public sealed class CustomerProjection(ICustomerReadStore store)
{
    private readonly ICustomerReadStore _store = store;

    public async Task ProjectAsync(IReadOnlyList<DomainEvent> events, CancellationToken ct)
    {
        foreach (var e in events)
        {
            switch (e)
            {
                case CustomerCreatedEvent created:
                    await _store.UpsertAsync(new CustomerReadDto(
                        Id: created.Id,
                        FirstName: created.FirstName,
                        LastName: created.LastName,
                        DateOfBirth: created.DateOfBirth,
                        PhoneNumber: created.PhoneNumber,
                        Email: created.Email,
                        BankAccountNumber: created.BankAccountNumber
                    ), ct);
                    break;

                case CustomerEmailChangedEvent changed:
                    var existing = await _store.GetByIdAsync(changed.Id, ct);
                    if (existing is null) break;

                    await _store.UpsertAsync(existing with { Email = changed.NewEmail }, ct);
                    break;
            }
        }
    }
}