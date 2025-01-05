namespace dbank.Domain.Entities
{
    public class PaymentEntity : BaseEntity
    {
        public long? CustomerId { get; set; }
        public CustomerEntity? Customer { get; set; }
        public string? Name { get; set; }
        public string? RecipientCardNumber { get; set; }
        public decimal? PaymentAmount { get; set; }     
    }
}
