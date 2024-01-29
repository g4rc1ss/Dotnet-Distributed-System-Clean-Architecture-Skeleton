using Infraestructure.Communication;
using Infraestructure.Communication.Consumers;
using Infraestructure.Communication.Consumers.Handler;
using Infraestructure.Communication.Messages;
using Infraestructure.Communication.Publisher;
using Infraestructure.RabbitMQ.Consumer;
using Infraestructure.RabbitMQ.Publisher;
using Infraestructure.RabbitMQ.Serialization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace Infraestructure.RabbitMQ;

public static class RabbitMqExtensions
{
    public static IServiceCollection AddRabbitMQ(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<RabbitMqSettings>(configuration.GetSection("RabbitMqData"));
        //services.AddRabbitMqHealthCheck();
        services.AddTransient<ISerializer, Serializer>();

        return services;
    }

    public static void AddRabbitMqConsumer<TMessage>(this IServiceCollection services)
    {
        services.AddConsumer<TMessage>();
        services.AddSingleton<IMessageConsumer<TMessage>, RabbitMqMessageConsumer<TMessage>>();
    }

    public static void AddRabbitMqPublisher<TMessage>(this IServiceCollection services)
        where TMessage : IMessage
    {
        services.AddPublisher<TMessage>();
        services.AddSingleton<IExternalMessagePublisher<TMessage>, RabbitMqPublisher<TMessage>>();
    }

    private static void AddRabbitMqHealthCheck(this IServiceCollection services)
    {
        services.AddHealthChecks()
        .AddRabbitMQ((serviceProvider, rabbitOptions) =>
        {
            var settings = serviceProvider.GetRequiredService<IOptions<RabbitMqSettings>>().Value;

            rabbitOptions.ConnectionFactory = new ConnectionFactory
            {
                UserName = settings.Credentials?.Username,
                Password = settings.Credentials?.Password,
                VirtualHost = "/",
                HostName = settings.Hostname,
                Port = AmqpTcpEndpoint.UseDefaultPort,
            };
            rabbitOptions.Connection = rabbitOptions.ConnectionFactory.CreateConnection();
        }, string.Empty, HealthStatus.Unhealthy);
    }


}

