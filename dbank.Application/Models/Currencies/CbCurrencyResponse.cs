namespace DBank.Application.Models.Currencies;

public class CbCurrencyResponse
{
    public DateTime Date { get; set; }
    public DateTime PreviousDate { get; set; }
    public DateTime Timestamp { get; set; }
    public ValuteDto? Valute { get; set; }
}
