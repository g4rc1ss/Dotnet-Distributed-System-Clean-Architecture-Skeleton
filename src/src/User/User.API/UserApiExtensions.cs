using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace User.API;

public static class UserApiExtensions
{
    public static IServiceCollection InitUser(this IServiceCollection services, IConfiguration configuration)
    {

        return services;
    }
}

