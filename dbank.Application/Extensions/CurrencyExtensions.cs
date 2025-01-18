using dbank.Application.Models.Сurrencies;
using dbank.Domain.Exceptions;

namespace dbank.Application.Extensions;

public static class CurrencyExtensions
{
    public static ValuteItemDto SearchCurrency(this ValuteDto valute, string currencyCode)
    {
        return currencyCode switch
        {
            "USD" => valute.USD,
            "EUR" => valute.EUR,
            "JPY" => valute.JPY,
            "KRW" => valute.KRW,
            "CNY" => valute.CNY,
            "AED" => valute.AED,
            "CAD" => valute.CAD,
            "THB" => valute.THB,
            _ => throw new CurrencyException("Currency not found.")
        };
    }
}
