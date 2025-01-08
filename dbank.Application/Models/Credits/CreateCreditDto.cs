namespace dbank.Application.Models.Credits;

public class CreateCreditDto
{
    public long? CustomerId { get; set; }
    public decimal? CreditAmount { get; set; }
    public decimal? CreditPeriod { get; set; }
}
