using System.ComponentModel.DataAnnotations;

namespace DBank.Domain.Entities
{
    public class TransactionEntity : BaseEntity
    {
        public long? CustomerId { get; set; }
        public CustomerEntity? Customer { get; set; }
        public required string Name { get; set; }
        public required string RecipientCard { get; set; }
        public decimal TransactionAmount { get; set; }     
    }
}
