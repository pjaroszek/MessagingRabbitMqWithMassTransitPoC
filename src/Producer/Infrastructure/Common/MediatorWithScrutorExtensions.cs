using System.Reflection;
using Jaroszek.CoderHouse.MessagingRabbitMqWithMassTransitPoC.Producer.Infrastructure.Common.Interfaces.Mediator;
using Microsoft.Extensions.DependencyInjection;

namespace Jaroszek.CoderHouse.MessagingRabbitMqWithMassTransitPoC.Producer.Infrastructure.Common;

public static class MediatorWithScrutorExtensions
{
    public static IServiceCollection AddMediator(this IServiceCollection services)
    {
      services.AddMediator(
            typeof(ICommand).Assembly  // Assembly z Application layer
        );

        return services;
    }
    
    public static void AddMediator(this IServiceCollection services, params Assembly[] assemblies)
    {
        services.AddScoped<IMediator, Mediator>();

        // Rejestruje wszystkie handlery zapytań
        services.Scan(scan => scan
            .FromAssemblies(assemblies)
            .AddClasses(classes => classes.AssignableTo(typeof(IQueryHandler<,>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        // Rejestruje wszystkie handlery komend
        services.Scan(scan => scan
            .FromAssemblies(assemblies)
            .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());
        
        // Register request handlers (MediatR compatibility)
        services.Scan(scan => scan
            .FromAssemblies(assemblies)
            .AddClasses(classes => classes.AssignableTo(typeof(IRequestHandler<,>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());
    }
}