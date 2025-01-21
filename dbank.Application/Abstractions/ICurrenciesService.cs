using DBank.Application.Models.Currencies;

namespace DBank.Application.Abstractions;

public interface ICurrenciesService
{
    Task UpdateCurrenciesCache();
    Task<ValuteItemDto> GetByCurrencyCode(string currencyCode);
}
