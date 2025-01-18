namespace dbank.Application.Models.Сurrencies;

public class CbCurrenciesResponse
{
    public DateTime Date { get; set; }
    public DateTime PreviousDate { get; set; }
    public DateTime Timestamp { get; set; }
    public ValuteItemDto Valute { get; set; }
}