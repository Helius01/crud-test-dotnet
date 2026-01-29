using Customers.Application.ReadModel;
using MediatR;

namespace Customers.Application.Queries.GetCustomerById;

public sealed class GetCustomerByIdQueryHandler(ICustomerReadStore store) : IRequestHandler<GetCustomerByIdQuery, CustomerReadDto?>
{
    private readonly ICustomerReadStore _store = store;
    public Task<CustomerReadDto?> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
                                    => _store.GetByIdAsync(request.Id, cancellationToken);

}