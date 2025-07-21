using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using NSwag;
using NSwag.Generation.Processors;
using NSwag.Generation.Processors.Security;

namespace Freaks.WebApi.Common.Extensions;

/// <summary>
///     Расширения для подключения и настройки Swagger (NSwag) в проекте.
/// </summary>
public static class SwaggerExtensions
{
    /// <summary>
    ///     Регистрирует NSwag (OpenAPI) в контейнере зависимостей с базовой конфигурацией:
    ///     версия, название API, поддержка авторизации через Bearer-токен, автоматическое добавление описаний.
    /// </summary>
    /// <param name="services">Коллекция сервисов для регистрации зависимостей.</param>
    /// <returns>Обновлённая коллекция сервисов <see cref="IServiceCollection" />.</returns>
    public static IServiceCollection AddNSwag(this IServiceCollection services)
    {
        services.AddOpenApiDocument(options =>
        {
            options.DocumentName = "v1";
            options.Version = "v1";
            options.Title = "Freaks API";
            options.UseControllerSummaryAsTagDescription = true;

            // Добавляем схему авторизации через Bearer-токе
            options.AddSecurity(
                "Bearer",
                [],
                new OpenApiSecurityScheme
                {
                    Type = OpenApiSecuritySchemeType.ApiKey,
                    Name = "Authorization",
                    In = OpenApiSecurityApiKeyLocation.Header,
                    Description = "Введите JWT токен как: Bearer {your_token}",
                });

            options.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("Bearer"));
            options.OperationProcessors.Add(new OperationSummaryAndDescriptionProcessor());
        });

        return services;
    }

    /// <summary>
    ///     Подключает middleware для генерации Swagger-документа и интерфейса пользователя NSwag.
    ///     Должен быть вызван в <c>Program.cs</c> после <c>UseRouting()</c> и до <c>UseEndpoints()</c>.
    /// </summary>
    /// <param name="app">Конвейер конфигурации приложения.</param>
    /// <returns>Обновлённый конфигуратор <see cref="IApplicationBuilder" />.</returns>
    public static IApplicationBuilder UseNSwag(this IApplicationBuilder app)
    {
        app.UseOpenApi();
        app.UseSwaggerUi();

        return app;
    }
}