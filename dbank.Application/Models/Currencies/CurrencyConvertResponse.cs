namespace DBank.Application.Models.Currencies;

public class CurrencyConvertResponse
{
    public string? CurrencyCode { get; set; }
    public string? Name { get; set; }
    public decimal CurrencyCourse { get; set; }
    public decimal AmountRubles { get; set; }
    public decimal AmountInCurrency  { get; set; }
}
