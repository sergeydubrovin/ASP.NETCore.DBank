namespace dbank.Application.Models.CashDeposits;

public class CreateCashDepositDto
{
    public long? CustomerId { get; set; }
    public string? Name { get; set; }
    public decimal? DepositAmount { get; set; }
    public decimal? DepositPeriod { get; set; }
    public decimal? InterestRate { get; set; }
}
