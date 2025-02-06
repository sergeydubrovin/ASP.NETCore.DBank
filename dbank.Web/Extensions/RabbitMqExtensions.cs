using RabbitMQ.Client;

namespace DBank.Web.Extensions;

public static class RabbitMqExtensions
{
    public static async Task DeclareExchangeAsync(this IChannel channel, string exchangeName, string exchangeType, 
                                                  CancellationToken stoppingToken)
    {
        await channel.ExchangeDeclareAsync(
            exchange: exchangeName,
            type: exchangeType,
            durable: true,
            autoDelete: false,
            arguments: null,
            cancellationToken: stoppingToken
        );
    }

    public static async Task DeclareQueueAsync(this IChannel channel, string queueName,
                                               CancellationToken stoppingToken)
    {
        await channel.QueueDeclareAsync(
            queue: queueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null,
            cancellationToken: stoppingToken
        );
    }

    public static async Task BindQueueAsync(this IChannel channel, string queueName, string exchangeName, 
                                            CancellationToken stoppingToken)
    {
        await channel.QueueBindAsync(
            queue: queueName,
            exchange: exchangeName,
            routingKey: queueName,
            cancellationToken: stoppingToken
        );
    }

    public static async Task Migrate(this IChannel channel, string exchangeName, string exchangeType, 
                                       string queueName, CancellationToken stoppingToken)
    {
        await channel.DeclareExchangeAsync(exchangeName, exchangeType, stoppingToken);
        
        await channel.DeclareQueueAsync(queueName, stoppingToken);
        await channel.BindQueueAsync(queueName, exchangeName, stoppingToken);
    }
}
