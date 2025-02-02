using System.Text;
using DBank.Application.Abstractions;
using DBank.Domain.Entities;
using DBank.Domain.Options;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace DBank.Application.Services;

public class RabbitMqProducer : IRabbitMqProducer
{
    private readonly RabbitMqOptions _rabbitMqOptions;
    private readonly IChannel _channel;

    public RabbitMqProducer(IOptions<RabbitMqOptions> options)
    {
        _rabbitMqOptions = options.Value;
        var factory = new ConnectionFactory
        {
            HostName = _rabbitMqOptions.Host,
            Port = _rabbitMqOptions.Port,
            UserName = _rabbitMqOptions.Username,
            Password = _rabbitMqOptions.Password,
            VirtualHost = _rabbitMqOptions.VirtualHost,
        };
        
        var connection = factory.CreateConnectionAsync().GetAwaiter().GetResult();
        _channel = connection.CreateChannelAsync().GetAwaiter().GetResult();
    }
    
    public async Task PublishTransactionToRabbitMq(string transactionData, string email)
    {
        var message = $"{email}|Транзакция|{transactionData}";
        var body = Encoding.UTF8.GetBytes(message);
        var properties = new BasicProperties
        {
            DeliveryMode = DeliveryModes.Persistent,
        };
        await _channel.BasicPublishAsync(exchange: _rabbitMqOptions.ExchangeName, routingKey: _rabbitMqOptions.QueueNameTransact, 
                                          mandatory: true, basicProperties: properties, body.AsMemory());
    } 

    public async Task PrepareTransactionMessage(CustomerEntity sender, CustomerEntity recipient, decimal amount)
    {
        var transactionData = $"VISA{recipient.Card[12..]} {DateTime.Now:HH:mm} " +
                              $"Перевод {amount}р от {sender.FirstName} {sender.MiddleName[..1]}.";
        
        await PublishTransactionToRabbitMq(transactionData, recipient.Email);
    }

    public async Task PublishWelcomeToRabbitMq(string welcomeData, string email)
    {
        var message = $"{email}|Добро пожаловать в DBank!|{welcomeData}";
        var body = Encoding.UTF8.GetBytes(message);
        var properties = new BasicProperties
        {
            DeliveryMode = DeliveryModes.Persistent,
        };
        await _channel.BasicPublishAsync(exchange: _rabbitMqOptions.ExchangeName, routingKey: _rabbitMqOptions.QueueNameWelcome,
                                         mandatory: true, basicProperties: properties, body.AsMemory());
    }

    public async Task PrepareWelcomeMessage(CustomerEntity customer)
    {
        var welcomeData = $"{customer.MiddleName} {customer.FirstName}{_rabbitMqOptions.WelcomeMessage}";
        await PublishWelcomeToRabbitMq(welcomeData, customer.Email);
    }

    public async Task PublishVerificationToRabbitMq(string verificationData, string email)
    {
        var message = $"{email}|Код для подтверждения почты|{verificationData}";
        var body = Encoding.UTF8.GetBytes(message);
        var properties = new BasicProperties
        {
            DeliveryMode = DeliveryModes.Persistent,
        };
        await _channel.BasicPublishAsync(exchange: _rabbitMqOptions.ExchangeName, routingKey: _rabbitMqOptions.QueueNameWelcome,
                                         mandatory: true, basicProperties: properties, body.AsMemory());
    }

    public async Task PrepareVerificationMessage(string verificationCode, string customerEmail)
    {
        var verificationData = $"Ваш код для подтверждения почты: {verificationCode}. " +
                               $"\nОбратите внимание, код действует 8 минут!";
        await PublishVerificationToRabbitMq(verificationData, customerEmail);
    }
}
