using DBank.Application.Models.Customers;

namespace DBank.Application.Abstractions;

public interface ICustomersService
{
    Task Create(CreateCustomerDto customer);
    Task<CustomerDto> GetById(long customerId);
}
