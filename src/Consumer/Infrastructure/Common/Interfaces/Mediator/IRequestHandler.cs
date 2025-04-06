namespace Jaroszek.CoderHouse.MessagingRabbitMqWithMassTransitPoC.Consumer.Infrastructure.Common.Interfaces.Mediator;

public interface IRequestHandler<in TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    Task<TResponse> HandleAsync(TRequest request, CancellationToken cancellationToken = default);
}