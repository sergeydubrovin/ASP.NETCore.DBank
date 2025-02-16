namespace DBank.Domain.Entities
{
    public class CustomerEntity : BaseEntity
    {
        public long? CustomerId { get; set; }
        public required string Phone { get; set; }
        public required string FirstName { get; set; } 
        public required string LastName { get; set; } 
        public required string MiddleName { get; set; }
        public required string Email { get; set; }
        public DateTime? BirthDate { get; set; }
        public bool IsVerified { get; set; }
        
        public long? UserId { get; set; }
        public UserEntity? User { get; set; }
        public CardEntity? Card { get; set; }
        public BalanceEntity? Balance { get; set; }
        public List<TransactionEntity>? Transactions { get; set; }
        public List<CashDepositEntity>? CashDeposits { get; set; }
        public List<CreditEntity>? Credits { get; set; }
    }
}
