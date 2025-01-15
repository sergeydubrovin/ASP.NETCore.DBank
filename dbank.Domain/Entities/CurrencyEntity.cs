namespace dbank.Domain.Entities;

public class CurrencyEntity : BaseEntity
{ 
    public decimal Usd { get; set; }
    public decimal Eur { get; set; }
    public decimal Jpy { get; set; }
    public DateTime UpdatedDate { get; set; }
}
