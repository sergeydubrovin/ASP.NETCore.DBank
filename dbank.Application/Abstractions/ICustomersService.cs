using dbank.Application.Models.Customers;

namespace dbank.Application.Abstractions;

public interface ICustomersService
{
    Task Create(CreateCustomerDto customer);
    Task<CustomerDto> GetById(long customerId);
}
