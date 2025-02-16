namespace DBank.Application.Models.Customers;

public class CreateCustomerDto
{
    public long CustomerId { get; set; }
    public required string Card { get; set; }
    public required string Phone { get; set; }
    public required string MiddleName { get; set; }
    public required string FirstName { get; set; } 
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public DateTime? BirthDate { get; set; }
}
