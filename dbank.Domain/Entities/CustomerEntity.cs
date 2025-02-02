namespace DBank.Domain.Entities
{
    public class CustomerEntity : BaseEntity
    {
        public long? CustomerId { get; set; }
        public required string Card { get; set; }
        public required string Phone { get; set; }
        public required string FirstName { get; set; } 
        public required string LastName { get; set; } 
        public required string MiddleName { get; set; }
        public string Email { get; set; }
        public DateTime? BirthDate { get; set; }
        
        public BalanceEntity? Balance { get; set; }
        public List<TransactionEntity>? Transactions { get; set; }
        public List<CashDepositEntity>? CashDeposits { get; set; }
        public List<CreditEntity>? Credits { get; set; }
    }
}
