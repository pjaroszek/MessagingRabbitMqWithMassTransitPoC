using Jaroszek.CoderHouse.Shared.Contracts;
using MassTransit;

namespace Jaroszek.CoderHouse.MessagingRabbitMqWithMassTransitPoC.Producer.Infrastructure;

public class MessageService
{
    private readonly IBus _bus;
    
    public MessageService(IBus bus)
    {
        _bus = bus;
    }
    
    public async Task SendMessage(string content)
    {
        await _bus.Publish<IYourMessage>(new 
        {
            Id = Guid.NewGuid(),
            Content = content,
            Timestamp = DateTime.UtcNow
        });
    }
}
