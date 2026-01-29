namespace Customers.Application.ReadModel;

public sealed record CustomerReadDto(
    Guid Id,
    string FirstName,
    string LastName,
    DateOnly DateOfBirth,
    string PhoneNumber,
    string Email,
    string BankAccountNumber
);
