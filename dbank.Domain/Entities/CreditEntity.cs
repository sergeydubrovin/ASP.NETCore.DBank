namespace DBank.Domain.Entities;

public class CreditEntity : BaseEntity
{
    public long? CustomerId { get; set; }
    public CustomerEntity? Customer { get; set; }
    public decimal? CreditAmount { get; set; }
    public int? CreditPeriod { get; set; }
    
    /// <summary>
    /// Процентная ставка кредита%
    /// </summary>
    public decimal? InterestCreditRate { get; set; } 
    
    /// <summary>
    /// Процентная ставка первоначального взноса%
    /// </summary>
    public decimal? InitialPaymentRate { get; set; } 
    
    /// <summary>
    /// Первоначальный взнос
    /// </summary>
    public decimal? InitialPayment { get; set; } 
    
    /// <summary>
    /// Ежемесячный платеж
    /// </summary>
    public decimal? MonthlyPayment { get; set; }
}
