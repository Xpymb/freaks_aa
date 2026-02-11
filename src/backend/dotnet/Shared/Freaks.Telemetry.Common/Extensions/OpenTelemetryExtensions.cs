using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using OpenTelemetry.Exporter;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Freaks.Telemetry.Common.Extensions;

public static class OpenTelemetryExtensions
{
    public static IServiceCollection AddMetricsAndTraces(this IServiceCollection services, IWebHostEnvironment environment, IConfiguration configuration)
    {
        var otlpEndpoint = configuration["OpenTelemetry:OtlpEndpoint"]?.TrimEnd('/');

        services.AddOpenTelemetry()
            .ConfigureResource(resource => resource
                .AddService(serviceName: environment.ApplicationName)
                .AddAttributes(new Dictionary<string, object>
                {
                    ["deployment_environment"] = environment.EnvironmentName.ToLower()
                }))
            .WithMetrics(metrics =>
            {
                metrics
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddRuntimeInstrumentation()
                    .AddProcessInstrumentation()
                    .AddEventCountersInstrumentation()
                    .AddNpgsqlInstrumentation();

                metrics.AddOtlpExporter(options =>
                {
                    options.Protocol = OtlpExportProtocol.HttpProtobuf;
                    if (!string.IsNullOrEmpty(otlpEndpoint))
                        options.Endpoint = new Uri($"{otlpEndpoint}/v1/metrics");
                });
            })
            .WithTracing(tracing => tracing
                .SetSampler<AlwaysOnSampler>()
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .AddNpgsql()
                .AddEntityFrameworkCoreInstrumentation()
                .AddOtlpExporter(options =>
                {
                    options.Protocol = OtlpExportProtocol.HttpProtobuf;
                    if (!string.IsNullOrEmpty(otlpEndpoint))
                        options.Endpoint = new Uri($"{otlpEndpoint}/v1/traces");
                }));

        return services;
    }
}
