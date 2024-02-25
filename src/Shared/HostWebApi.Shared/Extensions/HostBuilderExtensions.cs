using Microsoft.Extensions.Hosting;

using Serilog;
using Serilog.Events;

namespace HostWebApi.Shared.Extensions;

public static class HostBuilderExtensions
{

    public static IHostBuilder AddLoggerConfiguration(this IHostBuilder hostBuilder)
    {
        hostBuilder.UseSerilog((context, loggerConfiguration) =>
        {
            loggerConfiguration
                .MinimumLevel.Information()
                .Enrich.WithProperty("Application", context.Configuration["AppName"]!)
                .WriteTo.OpenTelemetry(options => options.Endpoint = context.Configuration["ConnectionStrings:OpenTelemetry"]!);

            if (context.HostingEnvironment.IsDevelopment())
            {
                loggerConfiguration.WriteTo.Console(LogEventLevel.Debug);
            };

        });
        return hostBuilder;
    }
}
