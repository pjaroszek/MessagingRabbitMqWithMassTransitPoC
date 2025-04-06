using Jaroszek.CoderHouse.Shared.Contracts;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Jaroszek.CoderHouse.MessagingRabbitMqWithMassTransitPoC.Consumer.Infrastructure;

public class MessageConsumer : IConsumer<IYourMessage>
{
    private readonly ILogger<MessageConsumer> _logger;

    public MessageConsumer(ILogger<MessageConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<IYourMessage> context)
    {
        _logger.LogInformation("Otrzymano wiadomość: {Id}, {Content}, {Timestamp}",
            context.Message.Id,
            context.Message.Content,
            context.Message.Timestamp);

        // Tutaj umieść logikę przetwarzania wiadomości

        return Task.CompletedTask;
    }
}