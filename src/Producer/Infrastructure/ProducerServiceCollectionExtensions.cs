using Jaroszek.CoderHouse.Shared.MassTransit;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Jaroszek.CoderHouse.MessagingRabbitMqWithMassTransitPoC.Producer.Infrastructure;

public static class ProducerServiceCollectionExtensions
{
    public static IServiceCollection AddMassTransit(this IServiceCollection services, IConfiguration configuration)
    {
        var rabbitMqOptions = configuration.GetSection(RabbitMqOptions.SectionName);
        var rabbitOptions = new RabbitMqOptions()
        {
            Server = new Uri(rabbitMqOptions.GetSection("Server").Value!.ToString()),
            Password = rabbitMqOptions.GetSection("Password").Value!.ToString(),
            User = rabbitMqOptions.GetSection("User").Value!.ToString()
        };
    
        services.AddMassTransit(configure =>
        {
           configure.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(rabbitOptions.Server, host =>
                {
                    host.Password(rabbitOptions.Password);
                    host.Username(rabbitOptions.User);
                });

                cfg.ConfigureEndpoints(context);
            });
        });
        services.AddMassTransitHostedService();

        return services;    
    }
    
    public static IServiceCollection AddMessageBus(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMassTransit(busConfig =>
        {
            busConfig.UsingRabbitMq((context, rabbitConfig) =>
            {
                rabbitConfig.Host(configuration["RabbitMQ:Host"], configuration["RabbitMQ:VirtualHost"], hostConfig =>
                {
                    hostConfig.Username(configuration["RabbitMQ:Username"]);
                    hostConfig.Password(configuration["RabbitMQ:Password"]);
                });
                
                // Opcjonalna konfiguracja endpointów
                rabbitConfig.ConfigureEndpoints(context);
            });
        });
        
        return services;
    }
}
