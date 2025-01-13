using dbank.Application.Abstractions;
using dbank.Application.Models.Сurrencies;
using dbank.Domain;
using dbank.Domain.Entities;
using dbank.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace dbank.Application.Services;

public class CurrenciesService(BankDbContext context) : ICurrenciesService
{
    public async Task Update(long currencyId, CreateCurrencyDto currency)
    {
        var entity = await context.Currencies.FindAsync(currencyId);

        if (entity == null)
        {
            entity = new CurrencyEntity
            {
                Id = currencyId,
                Usd = currency.Usd,
                Eur = currency.Eur,
                Jpy = currency.Jpy
            };
            context.Currencies.Add(entity);
        }
        else
        {
            entity.Usd = currency.Usd;
            entity.Eur = currency.Eur;
            entity.Jpy = currency.Jpy;
        }

        await context.SaveChangesAsync();
    }

    public async Task<CurrencyEntity> GetAll()
    {
        var currencies = await context.Currencies.FirstOrDefaultAsync();

        if (currencies == null)
        {
            throw new EntityNotFoundException("Курсы валют не найдены.");
        }
        
        return currencies;
    }
}
