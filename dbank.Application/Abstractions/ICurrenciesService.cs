using dbank.Application.Models.Сurrencies;
using dbank.Domain.Entities;

namespace dbank.Application.Abstractions;

public interface ICurrenciesService
{
    Task Update(long id, CreateCurrencyDto currency);
    Task<CurrencyEntity> GetAll();
}
