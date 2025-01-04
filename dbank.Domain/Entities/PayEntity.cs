namespace dbank.Domain.Entities
{
    public class PayEntity : BaseEntity
    {
        public CustomerEntity? Customer { get; set; }     
        public string? RecipientCardNumber { get; set; }
        public decimal? PaymentAmount { get; set; }
    }
}
