using DBank.Application.Models.Customers;

namespace DBank.Application.Abstractions;

public interface ICustomersService
{
    Task<long> Create(CreateCustomerDto customer);
    Task ValidateCode(VerificationDto verification);
    Task CompleteVerification(VerificationDto verification);
    Task<CustomerDto> GetById(long customerId);
}
