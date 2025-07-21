using Freaks.Dal.Common.Extensions;
using Freaks.Portal.Dal.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Freaks.Portal.Dal.Implementation;

/// <summary>
///     Расширения DI для Dal
/// </summary>
public static class ConfigureServices
{
    /// <summary>
    ///     Добавить Dal сервисы в DI
    /// </summary>
    /// <param name="services">Коллекция сервисов в DI</param>
    /// <param name="configuration">Конфигурация окружения</param>
    public static IServiceCollection AddDalProviders(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddPostgresDbContext<IPortalDbContext, PortalDbContext>(configuration);

        return services;
    }
}