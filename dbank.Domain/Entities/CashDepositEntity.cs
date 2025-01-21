namespace DBank.Domain.Entities;

public class CashDepositEntity : BaseEntity
{
    public long? CustomerId { get; set; }
    public CustomerEntity? Customer { get; set; }
    public string? Name { get; set; }
    public decimal? DepositAmount { get; set; }
    public int? DepositPeriod { get; set; }
    public decimal InterestRate { get; set; }   // процентная ставка%
    public decimal? FinalAmount { get; set; }
    public decimal? AccruedInterest  { get; set; }   // начисленные проценты%
}
