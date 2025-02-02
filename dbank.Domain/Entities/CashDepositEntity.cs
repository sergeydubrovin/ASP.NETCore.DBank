using System.ComponentModel.DataAnnotations;

namespace DBank.Domain.Entities;

public class CashDepositEntity : BaseEntity
{
    public long? CustomerId { get; set; }
    public CustomerEntity? Customer { get; set; }
    public string? Name { get; set; }
    public decimal? DepositAmount { get; set; }
    public int? DepositPeriod { get; set; }
    /// <summary>
    /// Процентная ставка%
    /// </summary>
    public decimal InterestRate { get; set; }
    
    public decimal? FinalAmount { get; set; }
    
    /// <summary>
    /// Начисленные проценты%
    /// </summary>
    public decimal? AccruedInterest  { get; set; }
}
