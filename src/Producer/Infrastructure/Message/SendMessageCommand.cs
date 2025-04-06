using Jaroszek.CoderHouse.MessagingRabbitMqWithMassTransitPoC.Producer.Infrastructure.Common.Interfaces.Mediator;
using Jaroszek.CoderHouse.Shared.Contracts;

namespace Jaroszek.CoderHouse.MessagingRabbitMqWithMassTransitPoC.Producer.Infrastructure.Message;


public record SendMessageCommand(string Content) : IRequest<IYourMessage>;