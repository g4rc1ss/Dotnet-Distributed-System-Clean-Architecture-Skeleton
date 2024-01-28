using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;

namespace HostWebApi.Shared.Extensions;

public static class HostBuilderExtensions
{

    public static IHostBuilder AddLoggerConfiguration(this IHostBuilder hostBuilder, IConfiguration configuration)
    {
        hostBuilder.UseSerilog((context, loggerConfiguration) =>
        {
            loggerConfiguration
                .MinimumLevel.Information()
                .Enrich.WithProperty("Application", configuration["AppName"]!)
                .WriteTo.Seq(configuration["ConnectionStrings:SeqLogs"]!);

            if (context.HostingEnvironment.IsDevelopment())
            {
                loggerConfiguration.WriteTo.Console(LogEventLevel.Debug);
            };

        });
        return hostBuilder;
    }
}
