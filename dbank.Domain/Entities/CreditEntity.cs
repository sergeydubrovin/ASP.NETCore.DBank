namespace DBank.Domain.Entities;

public class CreditEntity : BaseEntity
{
    public long? CustomerId { get; set; }
    public CustomerEntity? Customer { get; set; }
    public decimal? CreditAmount { get; set; }
    public int? CreditPeriod { get; set; }
    public decimal? InterestCreditRate { get; set; }   // процентная ставка%
    public decimal? InitialPaymentRate { get; set; }   // процентная ставка первоначального взноса%
    public decimal? InitialPayment { get; set; }   // первоначальный взнос
    public decimal? MonthlyPayment { get; set; }   // ежемесячный платеж
}
