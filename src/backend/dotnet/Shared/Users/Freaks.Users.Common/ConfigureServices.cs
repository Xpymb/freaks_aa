using Duende.AccessTokenManagement;
using Freaks.Dal.Common.Extensions;
using Freaks.Users.Bll.Implementations;
using Freaks.Users.Bll.Interfaces;
using Freaks.Users.Contracts.ValueObjects;
using Freaks.Users.Dal.Implementations;
using Freaks.Users.Dal.Interfaces;
using Freaks.Users.Dal.Persistence;
using Keycloak.AuthServices.Common;
using Keycloak.AuthServices.Sdk.Kiota;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using UserContextMiddleware = Freaks.Users.Common.Middlewares.UserContextMiddleware;

namespace Freaks.Users.Common;

/// <summary>
///     Методы расширения для регистрации и подключения контекста пользователя в приложение.
/// </summary>
public static class ConfigureServices
{
    /// <summary>
    ///     Регистрирует все зависимости, связанные с контекстом пользователя, включая провайдеры, сервисы, контекст БД и
    ///     доступ к <see cref="HttpContext" />.
    /// </summary>
    /// <param name="services">Коллекция сервисов приложения.</param>
    /// <param name="configuration">Конфигурация приложения для подключения к базе данных.</param>
    /// <returns>Обновлённая коллекция сервисов.</returns>
    public static IServiceCollection AddUserContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddKeycloakAdmin(configuration);
        
        services.AddPostgresDbContext<IUserDbContext, UserDbContext>(configuration);

        services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddScoped<IUserProvider, UserProvider>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IUserContext, UserContextAccessor>();

        return services;
    }

    /// <summary>
    ///     Регистрирует в DI-контейнере клиента администратора Keycloak с авторизацией по client_credentials.
    ///     Добавляет поддержку <c>IKeycloakRealmClient</c> для работы с Keycloak Admin API (realm-level).
    ///     Использует конфигурацию из секции <c>KeycloakAdmin</c> в <see cref="IConfiguration" />.
    /// </summary>
    /// <param name="services">Коллекция сервисов для регистрации зависимостей.</param>
    /// <param name="configuration">Конфигурация приложения, содержащая параметры подключения к Keycloak.</param>
    /// <returns>Коллекция сервисов <see cref="IServiceCollection" /> с зарегистрированным клиентом Keycloak.</returns>
    public static IServiceCollection AddKeycloakAdmin(this IServiceCollection services, IConfiguration configuration)
    {
        const string keycloakConfigSectionName = "KeycloakAdmin";
        const string tokenName = "keycloak-admin";

        var options = configuration.GetKeycloakOptions<KeycloakAdminClientOptions>(keycloakConfigSectionName)!;
        services.Configure<KeycloakAdminClientOptions>(configuration.GetSection(keycloakConfigSectionName));

        services.AddDistributedMemoryCache();
        services
            .AddClientCredentialsTokenManagement()
            .AddClient(
                tokenName,
                client =>
                {
                    client.ClientId = ClientId.Parse(options.Resource);
                    client.ClientSecret = ClientSecret.Parse(options.Credentials.Secret);
                    client.TokenEndpoint = new Uri(options.KeycloakTokenEndpoint);
                }
            );

        services
            .AddKeycloakAdminHttpClient(configuration, keycloakClientSectionName: keycloakConfigSectionName)
            .AddClientCredentialsTokenHandler(ClientCredentialsClientName.Parse(tokenName));
        
        return services;
    }

    /// <summary>
    ///     Подключает middleware, ответственный за наполнение контекста пользователя из текущего запроса. Подключать
    ///     обязательно после <c>UseAuthentification()</c>.
    /// </summary>
    /// <param name="app">Экземпляр приложения.</param>
    /// <returns>Обновлённый билдер приложения.</returns>
    public static IApplicationBuilder UseUserContext(this IApplicationBuilder app)
    {
        app.UseMiddleware<UserContextMiddleware>();

        return app;
    }
}