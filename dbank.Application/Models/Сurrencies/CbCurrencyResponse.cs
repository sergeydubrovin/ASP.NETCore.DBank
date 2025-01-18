namespace dbank.Application.Models.Сurrencies;

public class CbCurrencyResponse
{
    public DateTime Date { get; set; }
    public DateTime PreviousDate { get; set; }
    public DateTime Timestamp { get; set; }
    public ValuteDto Valute { get; set; }
}
