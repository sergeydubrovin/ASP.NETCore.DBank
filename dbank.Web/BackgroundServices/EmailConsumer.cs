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
    private readonly RabbitMqOptions _rabbitOptions;
    private IChannel _channel;
    private readonly IServiceProvider _serviceProvider;

    public EmailConsumer(IOptions<RabbitMqOptions> rabbitMqOptions, IServiceProvider serviceProvider)
    {
        _rabbitOptions = rabbitMqOptions.Value;
        _serviceProvider = serviceProvider;
        var factory = new ConnectionFactory
        {
            HostName = _rabbitOptions.Host,
            Port = _rabbitOptions.Port,
            UserName = _rabbitOptions.Username,
            Password = _rabbitOptions.Password,
            VirtualHost = _rabbitOptions.VirtualHost
        };
        
        var connection = factory.CreateConnectionAsync().GetAwaiter().GetResult();
        _channel = connection.CreateChannelAsync().GetAwaiter().GetResult();
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();

        var factory = new ConnectionFactory
        {
            HostName = _rabbitOptions.Host,
            Port = _rabbitOptions.Port,
            UserName = _rabbitOptions.Username,
            Password = _rabbitOptions.Password,
            VirtualHost = _rabbitOptions.VirtualHost,
        };
        var connection = factory.CreateConnectionAsync(stoppingToken).GetAwaiter().GetResult();
        _channel = connection.CreateChannelAsync(cancellationToken: stoppingToken).GetAwaiter().GetResult();
        
        await _channel.DeclareExchangeAsync(_rabbitOptions.ExchangeName, 
                                            _rabbitOptions.ExchangeType, stoppingToken);
        
        await _channel.DeclareQueueAsync(_rabbitOptions.QueueNameTransact, stoppingToken);
 
        await _channel.BindQueueAsync(_rabbitOptions.QueueNameTransact, _rabbitOptions.ExchangeName, 
                                      _rabbitOptions.QueueNameTransact, stoppingToken);

        await _channel.DeclareQueueAsync(_rabbitOptions.QueueNameWelcome, stoppingToken);

        await _channel.BindQueueAsync(_rabbitOptions.QueueNameWelcome, _rabbitOptions.ExchangeName, 
                                     _rabbitOptions.QueueNameWelcome, stoppingToken);
       
        
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
        
        await _channel.BasicConsumeAsync(queue: _rabbitOptions.QueueNameTransact, autoAck: false, 
                                         consumer: consumer, cancellationToken: stoppingToken);
        
        await _channel.BasicConsumeAsync(queue: _rabbitOptions.QueueNameWelcome, autoAck: false,
                                         consumer: consumer, cancellationToken: stoppingToken);
    }
}
