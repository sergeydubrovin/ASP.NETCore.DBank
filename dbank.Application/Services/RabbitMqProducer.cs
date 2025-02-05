using System.Text;
using System.Text.Json;
using DBank.Application.Abstractions;
using DBank.Application.Models.RabbitMq;
using DBank.Domain.Entities;
using DBank.Domain.Options;
using Microsoft.Extensions.Options;
using MimeKit;
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
        var message = new CreatePublishMessage
        {
            RecipientEmail = email,
            Subject = "Транзакция.",
            Body = transactionData
        };
        var jsonMessage = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(jsonMessage);
        
        var properties = new BasicProperties
        {
            DeliveryMode = DeliveryModes.Persistent,
        };
        await _channel.BasicPublishAsync(exchange: _rabbitMqOptions.ExchangeName, routingKey: _rabbitMqOptions.QueueNameTransact, 
                                          mandatory: true, basicProperties: properties, body.AsMemory());
    } 

    public async Task PrepareTransactionMessage(CreateTransactionMessage message)
    {
        var transactionData = $"VISA{message.RecipientCard[12..]} {DateTime.Now:HH:mm} " +
                              $"Перевод {message.TransactionAmount}р от {message.SenderFirstName} {message.SenderMiddleName[..1]}.";
        
        await PublishTransactionToRabbitMq(transactionData, message.RecipientEmail);
    }

    public async Task PublishWelcomeToRabbitMq(string welcomeData, string email)
    {
        var message = new CreatePublishMessage
        {
            RecipientEmail = email,
            Subject = "Добро пожаловать в DBank!",
            Body = welcomeData
        };
        var jsonMessage = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(jsonMessage);
        
        var properties = new BasicProperties
        {
            DeliveryMode = DeliveryModes.Persistent,
        };
        await _channel.BasicPublishAsync(exchange: _rabbitMqOptions.ExchangeName, routingKey: _rabbitMqOptions.QueueNameWelcome,
                                         mandatory: true, basicProperties: properties, body.AsMemory());
    }

    public async Task PrepareWelcomeMessage(CustomerEntity customer)
    {
        var welcomeMessage = _rabbitMqOptions.WelcomeMessage.Replace("{CustomerFio}", $"{customer.MiddleName} {customer.FirstName}");
        await PublishWelcomeToRabbitMq(welcomeMessage, customer.Email);
    }

    public async Task PublishVerificationToRabbitMq(string verificationData, string email)
    {
        var message = new CreatePublishMessage
        {
            RecipientEmail = email,
            Subject = "Код для подтверждения почты.",
            Body = verificationData
        };
        var jsonMessage = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(jsonMessage);
        
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
