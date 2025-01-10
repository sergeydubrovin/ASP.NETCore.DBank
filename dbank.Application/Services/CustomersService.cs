using dbank.Application.Abstractions;
using dbank.Application.Models.Customers;
using dbank.Domain;
using dbank.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace dbank.Application.Services;

public class CustomersService(BankDbContext context) : ICustomersService
{
    public async Task Create(CreateCustomerDto customer)
    {
        var entity = new CustomerEntity
        {
            CustomerId = customer.CustomerId,
            CardNumber = customer.CardNumber!,
            Phone = customer.Phone,
            MiddleName = customer.MiddleName,
            FirstName = customer.FirstName,
            LastName = customer.LastName,
            BirthDate = customer.BirthDate,
        };
        await context.Customers.AddAsync(entity);
        await context.SaveChangesAsync();
    }
    public async Task<CustomerEntity> GetById(long customerId)
    {
        var entity = await context.Customers.FirstOrDefaultAsync(e => e.Id == customerId);
        
        return entity;
    }
}
