using dbank.Application.Abstractions;
using dbank.Application.Models.CashDeposits;
using dbank.Domain;
using dbank.Domain.Entities;
using dbank.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace dbank.Application.Services;

public class CashDepositsService(BankDbContext context) : ICashDepositsService
{
    public async Task Create(CreateCashDepositDto deposit)
    {
        var entity = new CashDepositEntity
        {
            Name = deposit.DepositName,
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

        if (entity == null)
        {
            throw new EntityNotFoundException($"Вклад с id {depositId} не найден.");
        }

        return entity;
    }

    public async Task<List<CashDepositEntity>> GetByUser(long customerId)
    {
        var deposits = await context.CashDeposits.Where(d => d.CustomerId == customerId).ToListAsync();

        return deposits;
    }

    public async Task<List<CashDepositEntity>> GetAll()
    {
        var deposits = await context.CashDeposits.ToListAsync();
        
        return deposits;
    }
}
