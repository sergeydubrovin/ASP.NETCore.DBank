using System.ComponentModel.DataAnnotations;

namespace DBank.Application.Models.Payments
{
    public class CreatePaymentDto
    {
        public string? Name { get; set; }
        public long? CustomerId { get; set; }
        [StringLength(16, ErrorMessage = "Длина номера карты должны соответствовать 16 цифрам.")]
        public string? RecipientCardNumber { get; set; }
        public decimal PaymentAmount { get; set; }
    }
}
