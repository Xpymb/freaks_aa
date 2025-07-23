using Freaks.Files.Bll.Implementation.RaidFiles;
using Freaks.Files.Bll.Interfaces;
using Freaks.Files.Bll.Interfaces.RaidFiles;
using Microsoft.Extensions.DependencyInjection;

namespace Freaks.Files.Bll.Implementation;

/// <summary>
///     Класс-обёртка для регистрации бизнес-логики (BLL) в контейнере зависимостей.
/// </summary>
public static class ConfigureServices
{
    /// <summary>
    ///     Регистрирует сервисы уровня бизнес-логики (BLL) в контейнере зависимостей.
    /// </summary>
    /// <param name="services">Коллекция сервисов для регистрации зависимостей.</param>
    /// <returns>Модифицированная коллекция сервисов.</returns>
    public static IServiceCollection AddBllServices(this IServiceCollection services)
    {
        services.AddScoped<IRaidFileService, RaidScreenshotFileService>();

        services.AddScoped<IFileServiceFactory, FileServiceFactory>();

        return services;
    }
}