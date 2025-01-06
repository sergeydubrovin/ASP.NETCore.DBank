using dbank.Application.Models.Customers;
using dbank.Domain.Entities;

namespace dbank.Application.Abstractions;

public interface ICustomersService
{
    Task Create(CreateCustomerDto customer);
    Task<CustomerEntity> GetById(long customerId);
}
