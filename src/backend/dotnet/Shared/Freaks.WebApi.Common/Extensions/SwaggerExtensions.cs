using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using NSwag;
using NSwag.AspNetCore;
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

            options.AddSecurity(
                "Keycloak OAuth 2.0",
                [],
                new OpenApiSecurityScheme
                {
                    Type = OpenApiSecuritySchemeType.OAuth2,
                    Description = "Авторизация через Keycloak",
                    Flows =
                        new OpenApiOAuthFlows
                {
                    AuthorizationCode =
                        new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = "https://auth.freaks-aa.ru/realms/freaks-dev/protocol/openid-connect/auth",
                            TokenUrl = "https://auth.freaks-aa.ru/realms/freaks-dev/protocol/openid-connect/token",
                            Scopes =
                                new Dictionary<string, string>
                                {
                                    {
                                        "openid", "OpenID"
                                    },
                                    {
                                        "profile", "User profile"
                                    },
                                    {
                                        "email", "User email"
                                    },
                                },
                        },
                },
                });

            options.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("Keycloak OAuth 2.0"));
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
        app.UseSwaggerUi(options =>
        {
            options.OAuth2Client =
                new OAuth2ClientSettings
                {
                    ClientId = "swagger-ui",
                    AppName = "Freaks Swagger UI",
                    ScopeSeparator = " ",
                    Scopes =
                    {
                        "openid",
                        "profile",
                        "email",
                    },
                    UsePkceWithAuthorizationCodeGrant = true,
                };
        });

        return app;
    }
}