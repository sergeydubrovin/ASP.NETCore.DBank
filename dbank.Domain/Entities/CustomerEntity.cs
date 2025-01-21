namespace DBank.Domain.Entities
{
    public class CustomerEntity : BaseEntity
    {
        public long? CustomerId { get; set; }
        public string? CardNumber { get; set; }
        public string? Phone { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? MiddleName { get; set; }
        public DateTime? BirthDate { get; set; }
        
        public BalanceEntity? Balance { get; set; }
        public List<PaymentEntity>? Payments { get; set; }
        public List<CashDepositEntity>? CashDeposits { get; set; }
        public List<CreditEntity>? Credits { get; set; }
    }
}
