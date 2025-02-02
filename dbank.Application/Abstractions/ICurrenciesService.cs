using DBank.Application.Models.Currencies;

namespace DBank.Application.Abstractions;

public interface ICurrenciesService
{
    Task RefreshCurrencyCache();
    Task<ValuteItemDto> GetByCurrencyCode(string currencyCode);
    Task<CurrencyConvertResponse> CurrencyConverter(string currencyCode, decimal amountRubles);
}
