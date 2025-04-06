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
                // Rejestracja konsumenta
                busConfig.AddConsumer<MessageConsumer>();
            
                busConfig.UsingRabbitMq((context, rabbitConfig) =>
                {
                    // Konfiguracja połączenia RabbitMQ - rozwiązanie problemu null reference
                    var host = configuration["RabbitMQ:Host"] ?? "localhost";
                    var virtualHost = configuration["RabbitMQ:VirtualHost"] ?? "/";
                    var username = configuration["RabbitMQ:Username"] ?? "guest";
                    var password = configuration["RabbitMQ:Password"] ?? "guest";
                
                    rabbitConfig.Host(host, virtualHost, hostConfig =>
                    {
                        hostConfig.Username(username);
                        hostConfig.Password(password);
                    });
                
                    rabbitConfig.ReceiveEndpoint("message-coderHouse-exchange", endpointConfig =>
                    {
                        endpointConfig.ConfigureConsumeTopology = false;
                        endpointConfig.PrefetchCount = 1; // Obsługuj tylko jedną wiadomość naraz
                        endpointConfig.UseMessageRetry(retry => retry.Intervals(
                            TimeSpan.FromSeconds(10),  // pierwsza przerwa
                            TimeSpan.FromSeconds(10)   // druga przerwa
                        ));
                        // Rejestracja konsumenta
                        endpointConfig.ConfigureConsumer<MessageConsumer>(context);
                    });
                });
            });
        
            return services;
    }
}