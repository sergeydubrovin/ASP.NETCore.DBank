using DBank.Domain.Entities;

namespace DBank.Application.Models.RabbitMq;

public class CreateTransactionMessage
{
    public required string SenderFirstName { get; set; }
    public required string SenderMiddleName { get; set; }
    public required string RecipientCard { get; set; }
    public required string RecipientEmail { get; set; }
    public required decimal TransactionAmount { get; set; }
}
