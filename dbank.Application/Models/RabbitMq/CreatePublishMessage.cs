namespace DBank.Application.Models.RabbitMq;

public class CreatePublishMessage
{
    public required string RecipientEmail { get; set; }
    public required string Subject { get; set; }
    public required string Body { get; set; }
}
