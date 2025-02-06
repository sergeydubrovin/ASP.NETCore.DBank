namespace DBank.Domain.Options;

public class RabbitMqOptions
{
    public required string Host { get; init; } 
    public int Port { get; init; }
    public required string Username { get; init; }
    public required string Password { get; init; }
    public required string VirtualHost { get; init; }
    public required string QueueName { get; init; }
    public required string ExchangeName { get; init; }
    public required string ExchangeType { get; init; }
    
    public required string WelcomeMessage { get; init; }
}
