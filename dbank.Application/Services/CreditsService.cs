using DBank.Application.Abstractions;
using DBank.Application.Extensions;
using DBank.Application.Models.Credits;
using DBank.Domain;
using DBank.Domain.Entities;
using DBank.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace DBank.Application.Services;

public class CreditsService(BankDbContext context) : ICreditsService
{
    public async Task<CreditEntity> Create(CreateCreditDto credit)
    {
        var initialPayment = credit.ComputeInitialPayment();
        var monthlyPayment = credit.ComputeMonthlyPayment(initialPayment);
        
        var entity = new CreditEntity
        {
            CreditAmount = credit.CreditAmount,
            CreditPeriod = credit.CreditPeriod,
            CustomerId = credit.CustomerId,
            InterestCreditRate = credit.InterestRate,
            InitialPaymentRate = credit.InitialPaymentRate,
            InitialPayment = initialPayment,
            MonthlyPayment = monthlyPayment,
        };
        await context.Credits.AddAsync(entity);
        await context.SaveChangesAsync();
        
        return entity;
    }
    
    public async Task<CreditEntity> GetById(long creditId)
    {
        var entity = await context.Credits.FirstOrDefaultAsync(e => e.Id == creditId);

        if (entity == null)
        {
            throw new EntityNotFoundException($"Credit with id {creditId} not found.");
        }
        
        return entity;
    }
    
    public async Task<List<CreditEntity>> GetByUser(long customerId)
    {
        var credits = await context.Credits.Where(c => c.CustomerId == customerId).ToListAsync();

        return credits;
    }

    public async Task<List<CreditEntity>> GetAll()
    {
        var credits = await context.Credits.ToListAsync();
        
        return credits;
    }
}
