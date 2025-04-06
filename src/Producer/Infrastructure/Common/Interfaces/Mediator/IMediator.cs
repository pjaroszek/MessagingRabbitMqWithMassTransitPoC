namespace Jaroszek.CoderHouse.MessagingRabbitMqWithMassTransitPoC.Producer.Infrastructure.Common.Interfaces.Mediator;

public interface IMediator
{
    Task SendAsync(ICommand command, CancellationToken cancellationToken = default);
    Task<TResult> SendAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default);
    
    Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);
}