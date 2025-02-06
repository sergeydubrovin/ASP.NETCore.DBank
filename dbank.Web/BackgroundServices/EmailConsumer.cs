using System.Text;
using System.Text.Json;
using DBank.Application.Abstractions;
using DBank.Application.Models.Email;
using DBank.Application.Models.RabbitMq;
using DBank.Domain.Options;
using DBank.Web.Extensions;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace DBank.Web.BackgroundServices;

public class EmailConsumer : BackgroundService
{
    private readonly RabbitMqOptions _rOptions;
    private readonly IChannel _channel;
    private readonly IServiceProvider _serviceProvider;

    public EmailConsumer(IOptions<RabbitMqOptions> rabbitMqOptions, IServiceProvider serviceProvider)
    {
        _rOptions = rabbitMqOptions.Value;
        _serviceProvider = serviceProvider;
        var factory = new ConnectionFactory
        {
            HostName = _rOptions.Host,
            Port = _rOptions.Port,
            UserName = _rOptions.Username,
            Password = _rOptions.Password,
            VirtualHost = _rOptions.VirtualHost
        };
        
        var connection = factory.CreateConnectionAsync().GetAwaiter().GetResult();
        _channel = connection.CreateChannelAsync().GetAwaiter().GetResult();
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();
        
        await _channel.Migrate(_rOptions.ExchangeName, _rOptions.ExchangeType, _rOptions.QueueName, stoppingToken);
        
        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.ReceivedAsync += async (_, ea) =>
        {
            try
            {
                var body = ea.Body.ToArray();
                var jsonMessage = Encoding.UTF8.GetString(body);
                var message = JsonSerializer.Deserialize<CreatePublishMessage>(jsonMessage);
                
                var emailMessage = new CreateEmailMessage
                {
                    RecipientEmail = message!.RecipientEmail,
                    Subject = message.Subject,
                    Body = message.Body
                };
                    
                using var scope = _serviceProvider.CreateScope();
                var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();
                await emailService.SendEmail(emailMessage);
                
                await _channel.BasicAckAsync(ea.DeliveryTag, false, stoppingToken);
            }
            catch (Exception)
            {
                await _channel.BasicAckAsync(ea.DeliveryTag, false, stoppingToken);
            }
        };
        
        await _channel.BasicConsumeAsync(queue: _rOptions.QueueName, autoAck: false, 
                                         consumer: consumer, cancellationToken: stoppingToken);
    }
}
