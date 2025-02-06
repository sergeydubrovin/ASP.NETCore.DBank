namespace DBank.Domain.Entities;

public class CardEntity : BaseEntity
{
    public long? CustomerId { get; set; }
    public CustomerEntity? Customer { get; set; }
    public required string Card { get; set; }
}
