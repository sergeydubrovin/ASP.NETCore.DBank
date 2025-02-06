using DBank.Application.Models.RabbitMq;
using DBank.Domain.Entities;

namespace DBank.Application.Abstractions;

public interface IRabbitMqProducer
{
    Task PublishToRabbitMq(CreatePublishMessage message);
}
