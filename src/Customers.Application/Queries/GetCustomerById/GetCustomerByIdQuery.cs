using Customers.Application.ReadModel;
using MediatR;

namespace Customers.Application.Queries.GetCustomerById;

public sealed record GetCustomerByIdQuery(Guid Id) : IRequest<CustomerReadDto>;