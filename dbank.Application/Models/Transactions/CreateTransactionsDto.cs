using System.ComponentModel.DataAnnotations;

namespace DBank.Application.Models.Transactions
{
    public class CreateTransactionsDto
    {
        public string? Name { get; set; }
        public required long? CustomerId { get; set; }
        [StringLength(16, ErrorMessage = "Длина номера карты должны соответствовать 16 цифрам.")]
        public required string RecipientCard { get; set; }
        public required decimal TransactionAmount { get; set; }
    }
}
