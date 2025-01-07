using dbank.Application.Abstractions;
using dbank.Application.Models.CashDeposits;
using dbank.Domain;
using dbank.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace dbank.Application.Services;

public class CashDepositsService(BankDbContext context) : ICashDepositsService
{
    public async Task Create(CreateCashDepositDto deposit)
    {
        var entity = new CashDepositEntity
        {
            Name = deposit.Name,
            DepositAmount = deposit.DepositAmount,
            DepositPeriod = deposit.DepositPeriod,
            InterestRate = deposit.InterestRate,
            CustomerId = deposit.CustomerId,
        };
        await context.CashDeposits.AddAsync(entity);
        await context.SaveChangesAsync();
    }
    public async Task<CashDepositEntity> GetById(long depositId)
    {
        var entity = await context.CashDeposits.FirstOrDefaultAsync(e => e.Id == depositId);
        
        return entity;
    }

    public async Task<List<CashDepositEntity>> GetByUser(long customerId)
    {
        var deposits = await context.CashDeposits.Where(e => e.CustomerId == customerId).ToListAsync();
        
        return deposits;
    }
}
