namespace DBank.Application.Models.Customers;

public class VerificationDto
{
    public required string VerificationCode { get; set; }
    public required string CustomerId { get; set; }
}
