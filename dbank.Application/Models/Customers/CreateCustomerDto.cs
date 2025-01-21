namespace DBank.Application.Models.Customers;

public class CreateCustomerDto
{
    public long? CustomerId { get; set; }
    public string? CardNumber { get; set; }
    public string? Phone { get; set; }
    public string? MiddleName { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public DateTime? BirthDate { get; set; }
}
