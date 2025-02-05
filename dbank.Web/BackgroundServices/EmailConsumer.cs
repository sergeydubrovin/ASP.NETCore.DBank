using System.Text;
using DBank.Application.Abstractions;
using DBank.Application.Models.Email;
using DBank.Domain.Options;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace DBank.Web.BackgroundServices;

public class EmailConsumer : BackgroundService
{
    private readonly RabbitMqOptions _rabbitMqOptions;
    private IChannel _channel;
    private readonly IServiceProvider _serviceProvider;

    public EmailConsumer(IOptions<RabbitMqOptions> rabbitMqOptions, IServiceProvider serviceProvider)
    {
        _rabbitMqOptions = rabbitMqOptions.Value;
        _serviceProvider = serviceProvider;
        var factory = new ConnectionFactory
        {
            HostName = _rabbitMqOptions.Host,
            Port = _rabbitMqOptions.Port,
            UserName = _rabbitMqOptions.Username,
            Password = _rabbitMqOptions.Password,
            VirtualHost = _rabbitMqOptions.VirtualHost
        };
        
        var connection = factory.CreateConnectionAsync().GetAwaiter().GetResult();
        _channel = connection.CreateChannelAsync().GetAwaiter().GetResult();
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();

        var factory = new ConnectionFactory
        {
            HostName = _rabbitMqOptions.Host,
            Port = _rabbitMqOptions.Port,
            UserName = _rabbitMqOptions.Username,
            Password = _rabbitMqOptions.Password,
            VirtualHost = _rabbitMqOptions.VirtualHost,
        };
        var connection = factory.CreateConnectionAsync(stoppingToken).GetAwaiter().GetResult();
        _channel = connection.CreateChannelAsync(cancellationToken: stoppingToken).GetAwaiter().GetResult();
        
        await _channel.ExchangeDeclareAsync(
            exchange: _rabbitMqOptions.ExchangeName,
            type: _rabbitMqOptions.ExchangeType,
            durable: true,
            autoDelete: false,
            arguments: null,
            cancellationToken: stoppingToken
        );

        await _channel.QueueDeclareAsync(
            queue: _rabbitMqOptions.QueueNameTransact,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null,
            cancellationToken: stoppingToken
        );

        await _channel.QueueBindAsync(
            queue: _rabbitMqOptions.QueueNameTransact,
            exchange: _rabbitMqOptions.ExchangeName,
            routingKey: _rabbitMqOptions.QueueNameTransact,
            cancellationToken: stoppingToken
        );

        await _channel.QueueDeclareAsync(
            queue: _rabbitMqOptions.QueueNameWelcome,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null,
            cancellationToken: stoppingToken
        );

        await _channel.QueueBindAsync(
            queue: _rabbitMqOptions.QueueNameWelcome,
            exchange: _rabbitMqOptions.ExchangeName,
            routingKey: _rabbitMqOptions.QueueNameWelcome,
            cancellationToken: stoppingToken
        );
        
        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.ReceivedAsync += async (_, ea) =>
        {
            try
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                var messageParts = message.Split("|");
                var emailMessage = new CreateEmailMessage
                {
                    RecipientEmail = messageParts[0],
                    Subject = messageParts[1],
                    Body= messageParts[2]
                };
                    
                using var scope = _serviceProvider.CreateScope();
                var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();
                await emailService.SendEmail(emailMessage);
                
                await _channel.BasicAckAsync(ea.DeliveryTag, false, stoppingToken);
            }
            catch (Exception ex)
            {
                await _channel.BasicAckAsync(ea.DeliveryTag, false, stoppingToken);
            }
        };
        
        await _channel.BasicConsumeAsync(queue: _rabbitMqOptions.QueueNameTransact, autoAck: false, 
                                         consumer: consumer, cancellationToken: stoppingToken);
        
        await _channel.BasicConsumeAsync(queue: _rabbitMqOptions.QueueNameWelcome, autoAck: false,
                                         consumer: consumer, cancellationToken: stoppingToken);
    }
}
