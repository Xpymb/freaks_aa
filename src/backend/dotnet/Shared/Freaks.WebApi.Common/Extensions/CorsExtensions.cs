using Freaks.Options.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Freaks.WebApi.Common.Extensions;

/// <summary>
///     Расширения для настройки CORS в микросервисах.
/// </summary>
public static class CorsExtensions
{
    /// <summary>
    ///     Регистрирует CORS сервисы с настройками из конфигурации.
    /// </summary>
    /// <param name="services">Коллекция сервисов для регистрации зависимостей.</param>
    /// <param name="configuration">Конфигурация приложения.</param>
    /// <returns>Текущая коллекция сервисов для цепочки вызовов.</returns>
    public static IServiceCollection AddCorsWithConfiguration(this IServiceCollection services,
        IConfiguration configuration)
    {
        var corsOptions = configuration.GetSection("CorsOptions").Get<CorsOptions>() ?? new CorsOptions();

        services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                if (corsOptions.AllowedOrigins.Length == 0)
                    // Если origins не указаны, разрешаем все (только для Development/Compose)
                    policy.AllowAnyOrigin();
                else
                    // Используем указанные origins
                    policy.WithOrigins(corsOptions.AllowedOrigins);

                policy.WithMethods(corsOptions.AllowedMethods);
                policy.WithHeaders(corsOptions.AllowedHeaders);
                policy.WithExposedHeaders(corsOptions.ExposedHeaders);

                if (corsOptions.AllowCredentials) policy.AllowCredentials();
            });
        });

        return services;
    }
}