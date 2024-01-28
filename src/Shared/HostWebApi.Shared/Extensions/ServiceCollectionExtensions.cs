using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Exporter;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace HostWebApi.Shared.Extensions;

public static class ServiceCollectionExtensions
{

    public static IServiceCollection ConfigureDataProtectionProvider(this IServiceCollection services, IConfiguration configuration)
    {
        var keysFolder = configuration["keysFolder"]!;
        services.AddDataProtection()
            .PersistKeysToFileSystem(new DirectoryInfo(keysFolder))
            .SetApplicationName(configuration["AppName"]!);
        return services;
    }

    public static IServiceCollection AddOpenTelemetry(this IServiceCollection services, IConfiguration configuration,
        Action<MeterProviderBuilder> meterProvider = null, Action<TracerProviderBuilder> tracerProvider = null)
    {
        services.AddOpenTelemetry()
            .ConfigureResource(resource =>
            {
                resource.AddService(configuration["AppName"]!);
            })
            .WithTracing(trace =>
            {
                tracerProvider?.Invoke(trace);
                trace.AddAspNetCoreInstrumentation();
                trace.AddOtlpExporter(exporter =>
                {
                    exporter.Endpoint = new Uri(configuration["ConnectionStrings:OpenTelemetry"]!);
                });
            })
            .WithMetrics(metric =>
            {
                meterProvider?.Invoke(metric);
                metric.AddMeter(configuration["AppName"]!);
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

