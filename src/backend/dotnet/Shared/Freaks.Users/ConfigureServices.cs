using Freaks.Dal.Common.Extensions;
using Freaks.Users.Bll;
using Freaks.Users.Dal;
using Freaks.Users.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Freaks.Users;

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
        services.AddPostgresDbContext<IUserDbContext, UserDbContext>(configuration);

        services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddScoped<IUserProvider, UserProvider>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IUserContext, UserContextAccessor>();

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