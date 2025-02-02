namespace DBank.Application.Models.Currencies;

public class ValuteDto
{
    public required ValuteItemDto JPY { get; set; }
    public required ValuteItemDto USD { get; set; } 
    public required ValuteItemDto EUR { get; set; }
    public required ValuteItemDto KRW { get; set; }
    public required ValuteItemDto CNY { get; set; }
    public required ValuteItemDto AED { get; set; }
    public required ValuteItemDto CAD { get; set; }
    public required ValuteItemDto THB { get; set; }
}
