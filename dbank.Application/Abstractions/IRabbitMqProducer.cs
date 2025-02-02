using DBank.Domain.Entities;

namespace DBank.Application.Abstractions;

public interface IRabbitMqProducer
{
    Task PublishTransactionToRabbitMq(string transactionData, string email);
    Task PrepareTransactionMessage(CustomerEntity sender, CustomerEntity recipient, decimal amount);
    Task PublishWelcomeToRabbitMq(string welcomeData, string email);
    Task PrepareWelcomeMessage(CustomerEntity customer);
    Task PublishVerificationToRabbitMq(string verificationData, string email);
    Task PrepareVerificationMessage(string verificationCode, string customerEmail);
}
