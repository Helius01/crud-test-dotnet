using Customers.Application.Abstractions;
using Customers.Domain.Aggregates;
using Customers.Domain.ValueObjects;
using MediatR;

namespace Customers.Application.Commands.CreateCustomer;

public sealed class CreateCustomerHandler(IEventStore eventStore) : IRequestHandler<CreateCustomerCommand, Guid>
{
    private readonly IEventStore _eventStore = eventStore;
    public async Task<Guid> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = Customer.Create(
            id: request.Id,
            firstName: request.FirstName,
            lastName: request.LastName,
            dateOfBirth: request.DateOfBirth,
            phoneNumber: request.PhoneNumber,
            email: Email.Create(request.Email),
            bankAccountNumber: request.BankAccountNumber
        );

        var eventsToAppend = customer.UnCommittedEvents.ToList();

        await _eventStore.AppendToStreamAsync(
            streamId: request.Id,
            expectedVersion: 0,
            events: eventsToAppend,
            ct: cancellationToken
        );

        customer.ClearUncommittedEvents();
        return request.Id;

    }
}