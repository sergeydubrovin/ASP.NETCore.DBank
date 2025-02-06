namespace DBank.Application.Models.Cards;

public class CardDto
{
    public long? CustomerId { get; set; }
    public required string Card { get; set; }
}
