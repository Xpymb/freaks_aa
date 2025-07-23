using Freaks.Dal.Common.Extensions;
using Freaks.Files.Dal.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Freaks.Files.Dal.Implementation;

/// <summary>
///     Класс-обёртка для регистрации зависимостей уровня доступа к данным (DAL) в контейнере служб.
/// </summary>
public static class ConfigureServices
{
    /// <summary>
    ///     Регистрирует провайдеры доступа к данным в контейнере зависимостей.
    /// </summary>
    /// <param name="services">Коллекция сервисов для регистрации зависимостей.</param>
    /// <param name="configuration">Конфигурация приложения, содержащая необходимые параметры.</param>
    /// <returns>Модифицированная коллекция сервисов.</returns>
    public static IServiceCollection AddDalProviders(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMinioStorage(configuration);

        services.AddScoped<IStorageProvider, StorageProvider>();

        return services;
    }
}