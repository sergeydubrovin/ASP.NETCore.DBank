namespace dbank.Application.Models.Payments
{
    public class CreatePaymentDto
    {
        public string? Name { get; set; }
        public long? CustomerId { get; set; }
        public string? RecipientCardNumber { get; set; }
        public decimal? PaymentAmount { get; set; }
    }
}
