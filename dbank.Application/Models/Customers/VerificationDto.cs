namespace DBank.Application.Models.Customers;

public class VerificationResponse
{
    public required string VerificationCode { get; set; }
    public required string UserId { get; set; }
}
