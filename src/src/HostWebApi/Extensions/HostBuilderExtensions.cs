using OpenTelemetry.Resources;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using Serilog;
using Serilog.Events;
using StackExchange.Redis;
using OpenTelemetry.Exporter;

namespace HostWebApi.Extensions;

public static class HostBuilderExtensions
{

    internal static IHostBuilder AddLoggerConfiguration(this IHostBuilder hostBuilder, IConfiguration configuration)
    {
        hostBuilder.UseSerilog((context, loggerConfiguration) =>
        {
            loggerConfiguration
                .MinimumLevel.Information()
                .Enrich.WithProperty("Application", "HostWebApi")
                .WriteTo.Seq(configuration["ConnectionStrings:SeqLogs"]!);

            if (context.HostingEnvironment.IsDevelopment())
            {
                loggerConfiguration.WriteTo.Console(LogEventLevel.Debug);
            };

        });
        return hostBuilder;
    }

    internal static IServiceCollection AddOpenTelemetry(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOpenTelemetry()
            .ConfigureResource(resource =>
            {
                resource.AddService(configuration["AppName"]!);
            })
            .WithTracing(trace =>
            {
                trace.AddAspNetCoreInstrumentation();
                trace.AddEntityFrameworkCoreInstrumentation();
                trace.AddOtlpExporter(exporter =>
                {
                    exporter.Endpoint = new Uri(configuration["ConnectionStrings:OpenTelemetry"]!);
                });
            })
            .WithMetrics(metric =>
            {
                metric.AddMeter("HostWebApi");
                metric.AddAspNetCoreInstrumentation();
                metric.AddRuntimeInstrumentation();
                metric.AddHttpClientInstrumentation();
                metric.AddProcessInstrumentation();

                metric.AddOtlpExporter(exporter =>
                {
                    exporter.Endpoint = new Uri(configuration["ConnectionStrings:OpenTelemetry"]!);
                    exporter.Protocol = OtlpExportProtocol.Grpc;
                });
            });

        return services;
    }
}
