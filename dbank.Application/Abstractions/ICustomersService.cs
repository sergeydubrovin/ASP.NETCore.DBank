using DBank.Application.Models.Customers;

namespace DBank.Application.Abstractions;

public interface ICustomersService
{
    Task<string> Create(CreateCustomerDto customer);
    Task<bool> Verification(VerificationDto verification);
    Task Save(VerificationDto verification);
    Task<CustomerDto> GetById(long customerId);
}
