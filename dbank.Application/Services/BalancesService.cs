using dbank.Application.Abstractions;
using dbank.Application.Models.Balances;
using dbank.Domain;
using dbank.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace dbank.Application.Services;

public class BalancesService(BankDbContext context) : IBalancesService
{
    public async Task Create(CreateBalanceDto balance)
    {
        var entity = new BalanceEntity
        {
            Balance = balance.Balance,
            CustomerId = balance.CustomerId,
        };
        await context.Balances.AddAsync(entity);
        await context.SaveChangesAsync();
    }

    public async Task<BalanceEntity> GetByUser(long customerId)
    {
        var balance = await context.Balances.FirstOrDefaultAsync(b => b.CustomerId == customerId);

        return balance;
    }
}
