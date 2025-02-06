namespace DBank.Application.Models.Customers;

public class VerificationData
{
    public required string VerificationCode { get; set; }
    public required DateTime VerificationDate { get; set; }
    public required long CustomerId { get; set; }
}
