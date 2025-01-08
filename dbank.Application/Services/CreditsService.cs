using dbank.Application.Abstractions;
using dbank.Application.Models.Credits;
using dbank.Domain;
using dbank.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace dbank.Application.Services;

public class CreditsService(BankDbContext context) : ICreditsService
{
    public async Task Create(CreateCreditDto credit)
    {
        var entity = new CreditEntity
        {
            CreditAmount = credit.CreditAmount,
            CreditPeriod = credit.CreditPeriod,
            CustomerId = credit.CustomerId,
        };
        await context.Credits.AddAsync(entity);
        await context.SaveChangesAsync();
    }
    public async Task<CreditEntity> GetById(long creditId)
    {
        var entity = await context.Credits.FirstOrDefaultAsync(e => e.Id == creditId);
        
        return entity;
    }
    public async Task<List<CreditEntity>> GetByUser(long customerId)
    {
        var credits = await context.Credits.Where(c => c.CustomerId == customerId).ToListAsync();

        return credits;
    }
}
