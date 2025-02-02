using DBank.Application.Models.Customers;

namespace DBank.Application.Abstractions;

public interface ICustomersService
{
    Task<string> Create(CreateCustomerDto customer);
    Task<bool> Verify(string verificationCode, string userId);
    Task Save(string verificationCode, string userId);
    Task<CustomerDto> GetById(long customerId);
}
