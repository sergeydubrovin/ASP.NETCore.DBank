namespace DBank.Application.Models.Email;

public class CreateEmailMessage
{
    public required string RecipientEmail { get; set; }
    public required string Subject { get; set; }
    public required string Body { get; set; }
}
