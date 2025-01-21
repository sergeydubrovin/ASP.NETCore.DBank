using DBank.Application.Abstractions;
using DBank.Application.Mappers;
using DBank.Application.Models.Customers;
using DBank.Domain;
using DBank.Domain.Entities;
using DBank.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace DBank.Application.Services;

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
            .Include(c => c.CashDeposits)
            .Include(b => b.Balance)
            .Include(p => p.Payments)
            .Include(c => c.Credits)
            .FirstOrDefaultAsync(e => e.Id == customerId);

        if (entity == null)
        {
            throw new EntityNotFoundException($"Customer with id {customerId} not found.");
        }
        
        return entity.ToDto();
    }
}
