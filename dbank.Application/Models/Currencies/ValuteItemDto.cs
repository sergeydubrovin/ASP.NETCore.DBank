using System.Text.Json.Serialization;

namespace DBank.Application.Models.Currencies;

public class ValuteItemDto
{
    [JsonPropertyName("ID")]
    public required string Id { get; set; }
    
    public string? NumCode { get; set; }
    public string? CharCode { get; set; }
    public int Nominal  { get; set; }
    public string? Name { get; set; }
    public decimal Value { get; set; }
    public decimal Previous { get; set; }
}
