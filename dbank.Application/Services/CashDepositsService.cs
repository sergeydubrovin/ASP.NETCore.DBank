using DBank.Application.Abstractions;
using DBank.Application.Extensions;
using DBank.Application.Models.CashDeposits;
using DBank.Domain;
using DBank.Domain.Entities;
using DBank.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace DBank.Application.Services;

public class CashDepositsService(BankDbContext context) : ICashDepositsService
{
    public async Task<CashDepositEntity> Create(CreateCashDepositDto deposit)
    {
        deposit.ValidationCashDeposit();
        
        var finalAmount = deposit.ComputeFinalAmount();
        var accruedInterest = finalAmount - deposit.DepositAmount;
        
        var entity = new CashDepositEntity
        {
            Name = deposit.DepositName,
            DepositAmount = deposit.DepositAmount,
            DepositPeriod = deposit.DepositPeriod,
            InterestRate = deposit.InterestRate,
            CustomerId = deposit.CustomerId,
            FinalAmount = finalAmount,
            AccruedInterest = accruedInterest,
        };
        await context.CashDeposits.AddAsync(entity);
        await context.SaveChangesAsync();
        
        return entity;
    }

    public async Task<CashDepositEntity> GetById(long depositId)
    {
        var entity = await context.CashDeposits.FirstOrDefaultAsync(e => e.Id == depositId);

        if (entity == null)
        {
            throw new EntityNotFoundException($"CashDeposit with id {depositId} not found.");
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
