using DBank.Application.Models.Currencies;

namespace DBank.Application.Extensions;

public static class CurrencyExtensions
{
    public static CurrencyConvertResponse ComputeCurrency(this ValuteItemDto valute, decimal amountRubles)
    {
        var nominal = valute.Nominal;
        var previous = valute.Previous;
        
        var amountInCurrency = nominal switch
        {
            1 => amountRubles / previous,
            10 => (amountRubles / previous) * 10,
            100 => (amountRubles / previous) * 100,
            1000 => (amountRubles / previous) * 1000,
            _ => throw new ArgumentOutOfRangeException($"Nominal {nominal} is not supported."),
        };

        var currencyCourse = nominal switch
        {
            10 => previous / nominal,
            100 => previous / nominal,
            1000 => previous / nominal,
            _ => previous,
        };
        
        var response = new CurrencyConvertResponse
        {
            CurrencyCode = valute.CharCode,
            Name = valute.Name,
            CurrencyCourse = Math.Round(currencyCourse, 2),
            AmountRubles = amountRubles,
            AmountInCurrency = Math.Round(amountInCurrency, 2)
        };
        return response;
    }
}
