using Infraestructure.Communication.Consumers.Handler;
using Infraestructure.Communication.Consumers.Host;
using Infraestructure.Communication.Consumers.Manager;
using Infraestructure.Communication.Messages;
using Infraestructure.Communication.Publisher.Domain;
using Infraestructure.Communication.Publisher.Integration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Infraestructure.Communication;

public static class CommunicationExtensions
{
    public static void AddConsumer<TMessage>(this IServiceCollection services)
    {
        services.AddSingleton<IConsumerManager<TMessage>, ConsumerManager<TMessage>>();
        services.AddSingleton<IHostedService, ConsumerHostedService<TMessage>>();
    }

    public static void AddHandlersInAssembly<T>(this IServiceCollection services)
    {
        services.Scan(scan => scan.FromAssemblyOf<T>()
            .AddClasses(classes => classes.AssignableTo<IMessageHandler>())
            .AsImplementedInterfaces()
            .WithTransientLifetime());

        var sp = services.BuildServiceProvider();
        var listHandlers = sp.GetServices<IMessageHandler>();
        services.AddConsumerHandlers(listHandlers);
    }

    public static void AddConsumerHandlers(this IServiceCollection services, IEnumerable<IMessageHandler> messageHandlers)
    {
        services.AddSingleton<IMessageHandlerRegistry>(new MessageHandlerRegistry(messageHandlers));
        services.AddSingleton<IHandleMessage, HandleMessage>();
    }

    public static void AddPublisher<TMessage>(this IServiceCollection services)
    {
        if (typeof(TMessage) == typeof(IntegrationMessage))
        {
            services.AddIntegrationServiceBus();
        }
        else if (typeof(TMessage) == typeof(DomainMessage))
        {
            services.AddDomainServiceBus();
        }
    }

    private static void AddIntegrationServiceBus(this IServiceCollection services)
    {
        services.AddTransient<IIntegrationMessagePublisher, DefaultIntegrationMessagePublisher>();
    }

    private static void AddDomainServiceBus(this IServiceCollection services)
    {
        services.AddTransient<IDomainMessagePublisher, DefaultDomainMessagePublisher>();
    }
}

