using DBank.Application.Abstractions;
using DBank.Application.Models.RabbitMq;
using DBank.Domain.Entities;
using DBank.Domain.Options;
using Microsoft.Extensions.Options;

namespace DBank.Application.Services;

public class RabbitMqService(IRabbitMqProducer producer, IOptions<NotificationsOptions> notifications) : IRabbitMqService
{
    private readonly NotificationsOptions _notifications = notifications.Value;
    
    public async Task PrepareTransactionMessage(CreateTransactionMessage transact)
    {
        var message = new CreatePublishMessage
        {
            RecipientEmail = transact.RecipientEmail,
            Subject = "Транзакция.",
            Body = $"VISA{transact.RecipientCard[12..]} {DateTime.Now:HH:mm} " +
                   $"Перевод {transact.TransactionAmount}р от {transact.SenderFirstName} {transact.SenderMiddleName[..1]}."
        };
        
        await producer.PublishToRabbitMq(message);
    }
    
    public async Task PrepareWelcomeMessage(CustomerEntity customer)
    {
        var message = new CreatePublishMessage
        {
            RecipientEmail = customer.Email,
            Subject = "Добро пожаловать в DBank!",
            Body = _notifications.WelcomeMessage.Replace("{CustomerFio}", $"{customer.MiddleName} {customer.FirstName}")
        };
            
        await producer.PublishToRabbitMq(message);
    }
    
    public async Task PrepareVerificationMessage(string verificationCode, string customerEmail)
    {
        var message = new CreatePublishMessage
        {
            RecipientEmail = customerEmail,
            Subject = "Код для подтверждения почты.",
            Body = _notifications.VerificationMessage.Replace("{VerificationCode}", $"{verificationCode}")
        };
        
        await producer.PublishToRabbitMq(message);
    }
}
