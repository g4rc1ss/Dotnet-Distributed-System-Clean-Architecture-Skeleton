using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Infraestructure.ServiceBus;

public static class ServiceBusExtensions
{

    public static IServiceCollection AddServiceBusConfig(this IServiceCollection services)
    {
        // Agregamos mediatr, que se puede definir como un SB en Memoria
        services.AddMediatR(typeof(ServiceBusExtensions));
        services.AddAutoMapper(typeof(ServiceBusExtensions));

        return services;
    }

}

