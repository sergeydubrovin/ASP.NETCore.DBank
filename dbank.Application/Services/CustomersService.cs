using dbank.Application.Abstractions;
using dbank.Application.Mappers;
using dbank.Application.Models.Customers;
using dbank.Domain;
using dbank.Domain.Entities;
using dbank.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace dbank.Application.Services;

public class CustomersService(BankDbContext context) : ICustomersService
{
    public async Task Create(CreateCustomerDto customer)
    {
        var entity = new CustomerEntity
        {
            CustomerId = customer.CustomerId,
            CardNumber = customer.CardNumber,
            Phone = customer.Phone,
            MiddleName = customer.MiddleName,
            FirstName = customer.FirstName,
            LastName = customer.LastName,
            BirthDate = customer.BirthDate,
        };
        await context.Customers.AddAsync(entity);
        await context.SaveChangesAsync();
    }
    
    public async Task<CustomerDto> GetById(long customerId)
    {
        var entity = await context.Customers
            .Include(b => b.Balance)
            .FirstOrDefaultAsync(e => e.Id == customerId);

        if (entity == null)
        {
            throw new EntityNotFoundException($"Пользователь с id {customerId} на найден.");
        }
        
        return entity.ToDto();
    }
}
