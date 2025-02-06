using System.Text;
using System.Text.Json;
using DBank.Application.Abstractions;
using DBank.Application.Models.RabbitMq;
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
    
    public async Task PublishToRabbitMq(CreatePublishMessage message)
    {
        var jsonMessage = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(jsonMessage);
        
        var properties = new BasicProperties
        {
            DeliveryMode = DeliveryModes.Persistent,
        };
        await _channel.BasicPublishAsync(exchange: _rabbitMqOptions.ExchangeName, routingKey: _rabbitMqOptions.QueueName,
                                         mandatory: true, basicProperties: properties, body.AsMemory());
    }
}
