using System.Text.Json.Serialization;

namespace DBank.Application.Models.Customers;

public class CustomerDto : CreateCustomerDto
{
    [JsonIgnore]
    public long Id { get; set; }
    public decimal? Balance { get; set; }
    public int CashDepositsCount { get; set; }
    public int PaymentsCount { get; set; }
    public int CreditsCount { get; set; }
}
