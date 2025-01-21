namespace DBank.Application.Models.Balances;

public class CreateBalanceDto
{
    public long? CustomerId { get; set; }
    public decimal? Balance { get; set; }
}
