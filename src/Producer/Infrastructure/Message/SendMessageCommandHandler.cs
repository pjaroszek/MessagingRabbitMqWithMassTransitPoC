using Jaroszek.CoderHouse.MessagingRabbitMqWithMassTransitPoC.Producer.Infrastructure.Common.Interfaces.Mediator;
using Jaroszek.CoderHouse.Shared.Contracts;

namespace Jaroszek.CoderHouse.MessagingRabbitMqWithMassTransitPoC.Producer.Infrastructure.Message;

public class SendMessageCommandHandler(MessageService messageService)
    : IRequestHandler<SendMessageCommand, IYourMessage>
{
    public async Task<IYourMessage> HandleAsync(SendMessageCommand request, CancellationToken cancellationToken = default)
    {
        await messageService.SendMessage(request.Content);
        return  new YourMessage("Message sent");
    }
}