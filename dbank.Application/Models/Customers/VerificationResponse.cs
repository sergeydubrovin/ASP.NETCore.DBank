namespace DBank.Application.Models.Customers;

public class VerifyResponse
{
    public required string VerificationCode { get; set; }
    public required string UserId { get; set; }
}
