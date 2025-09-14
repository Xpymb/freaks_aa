using System.IO.Compression;
using Freaks.WebApi.Common.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Freaks.WebApi.Common.Extensions;

/// <summary>
///     Расширения для регистрации и настройки стандартных зависимостей и middleware.
/// </summary>
public static class DependencyInjectionExtensions
{
    /// <summary>
    ///     Регистрирует стандартные сервисы и зависимости в контейнере DI.
    ///     Используется для централизованного подключения общих модулей приложения.
    /// </summary>
    /// <param name="services">Коллекция сервисов для регистрации зависимостей.</param>
    /// <param name="configuration">Конфигурация приложения (например, из <c>appsettings.json</c>, переменных среды и т.д.).</param>
    /// <returns>Текущая коллекция сервисов <see cref="IServiceCollection" /> для цепочки вызовов.</returns>
    public static IServiceCollection AddDefaults(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddResponseCompression(options =>
        {
            options.EnableForHttps = true;
            options.Providers.Add<GzipCompressionProvider>();
        });

        services.Configure<GzipCompressionProviderOptions>(options =>
        {
            options.Level = CompressionLevel.Optimal;
        });

        services.AddHealthChecks();
        services.AddCorsWithConfiguration(configuration);

        return services;
    }

    /// <summary>
    ///     Подключает стандартные middleware-компоненты в конвейере обработки запросов.
    ///     Используется для настройки общих правил маршрутизации, базовых путей и т.д.
    /// </summary>
    /// <param name="app">Экземпляр приложения ASP.NET Core.</param>
    /// <returns>Конфигуратор приложения <see cref="IApplicationBuilder" /> для цепочки вызовов.</returns>
    public static IApplicationBuilder UseDefaults(this IApplicationBuilder app)
    {
        app.UseResponseCompression();

        app.UseHealthChecks("/healthz");
        app.UseCors();

        return app;
    }

    public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionHandlerMiddleware>();

        return app;
    }
}