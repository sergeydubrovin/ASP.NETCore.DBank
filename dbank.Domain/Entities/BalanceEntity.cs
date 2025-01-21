namespace DBank.Domain.Entities;

public class BalanceEntity : BaseEntity
{
    public long? CustomerId { get; set; }
    public CustomerEntity? Customer { get; set; }
    public decimal? Balance { get; set; }
}
