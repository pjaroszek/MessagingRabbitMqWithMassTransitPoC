﻿namespace Jaroszek.CoderHouse.MessagingRabbitMqWithMassTransitPoC.Producer.Infrastructure.Common.Interfaces.Mediator;

public interface IQueryHandler<in TQuery, TResult> where TQuery : IQuery<TResult>
{
    Task<TResult> HandleAsync(TQuery query, CancellationToken cancellationToken = default);
}