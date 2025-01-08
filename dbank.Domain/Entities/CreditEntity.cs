namespace dbank.Domain.Entities;

public class CreditEntity : BaseEntity
{
    public long? CustomerId { get; set; }
    public CustomerEntity? Customer { get; set; }
    public decimal? CreditAmount { get; set; }
    public decimal? CreditPeriod { get; set; }
    public decimal? InterestRate { get; set; }   // процентная ставка%
    public decimal? InitialPayment { get; set; }   // первоначальный взнос 
}
