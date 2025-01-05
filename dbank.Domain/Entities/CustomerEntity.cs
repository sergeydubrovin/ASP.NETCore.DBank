namespace dbank.Domain.Entities
{
    public class CustomerEntity : BaseEntity
    {
        public string? CardNumber { get; set; }
        public string? Phone { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? MiddleName { get; set; }
        public decimal? Balance { get; set; }
        public DateTime? BirthDate { get; set; }

        public List<PaymentEntity>? Payments { get; set; }
    }
}
