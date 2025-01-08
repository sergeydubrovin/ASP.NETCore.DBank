namespace dbank.Domain.Entities;

public class CashDepositEntity : BaseEntity
{
    public long? CustomerId { get; set; }
    public CustomerEntity? Customer { get; set; }
    public string? Name { get; set; }
    public decimal? DepositAmount { get; set; }
    public decimal? DepositPeriod { get; set; }
    public decimal? InterestRate { get; set; }   // процентная ставка%
}
