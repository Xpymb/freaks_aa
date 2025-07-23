using Freaks.Options.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minio;

namespace Freaks.Dal.Common.Extensions;

/// <summary>
///     Методы расширения для регистрации хранилища MinIO в контейнере зависимостей.
/// </summary>
public static class StorageExtensions
{
    /// <summary>
    ///     Добавляет и настраивает MinIO-клиент в контейнере зависимостей, используя параметры из конфигурации.
    /// </summary>
    /// <param name="services">Коллекция сервисов для регистрации зависимостей.</param>
    /// <param name="configuration">Конфигурация приложения, содержащая секцию StorageOptions.</param>
    /// <returns>Модифицированная коллекция сервисов.</returns>
    /// <exception cref="ArgumentNullException">Выбрасывается, если StorageOptions не найдены в конфигурации.</exception>
    public static IServiceCollection AddMinioStorage(this IServiceCollection services, IConfiguration configuration)
    {
        var storageOptionsSection = configuration.GetSection(nameof(StorageOptions));

        services.Configure<StorageOptions>(storageOptionsSection);
        var storageOptions = storageOptionsSection.Get<StorageOptions>();

        if (storageOptions is null)
        {
            throw new ArgumentNullException(nameof(storageOptions));
        }

        services.AddMinio(config =>
                              config.WithEndpoint(storageOptions.Host)
                                    .WithCredentials(storageOptions.AccessKey, storageOptions.SecretKey)
                                    .WithRegion(storageOptions.Region)
                                    .WithSSL(false)
                                    .Build());

        return services;
    }
}