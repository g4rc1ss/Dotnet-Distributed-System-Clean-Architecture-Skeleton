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

