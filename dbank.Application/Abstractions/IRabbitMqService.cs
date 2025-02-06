using DBank.Application.Models.RabbitMq;
using DBank.Domain.Entities;

namespace DBank.Application.Abstractions;

public interface IRabbitMqService
{
    Task PrepareTransactionMessage(CreateTransactionMessage transact);
    Task PrepareWelcomeMessage(CustomerEntity customer);
    Task PrepareVerificationMessage(string verificationCode, string customerEmail);
}
