using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Jaroszek.CoderHouse.MessagingRabbitMqWithMassTransitPoC.Consumer.Infrastructure;

public static class ConsumerServiceCollectionExtensions
{
    public static IServiceCollection AddMessageBus(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMassTransit(busConfig =>
        {
            // Dodaj obsługę konsumenta
            busConfig.AddConsumer<MessageConsumer>();
            
            busConfig.UsingRabbitMq((context, rabbitConfig) =>
            {
                rabbitConfig.Host(configuration["RabbitMQ:Host"], configuration["RabbitMQ:VirtualHost"], hostConfig =>
                {
                    hostConfig.Username(configuration["RabbitMQ:Username"]);
                    hostConfig.Password(configuration["RabbitMQ:Password"]);
                });
                
                // Konfiguracja kolejek i konsumentów
                rabbitConfig.ReceiveEndpoint("your-queue-name", endpointConfig =>
                {
                    endpointConfig.ConfigureConsumer<MessageConsumer>(context);
                });
            });
        });
        
        return services;
    }
}