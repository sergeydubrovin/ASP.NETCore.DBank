namespace DBank.Domain.Options;

public class CbOptions
{
    public string BaseUrl { get; set; } = null!;
    public string CurrencyPath { get; set; } = null!;
    public int DelayMinutes { get; set; }
    public int CacheDurationHours { get; set; }
    public List<string> SupportedCurrencies { get; set; } = null!;
}
