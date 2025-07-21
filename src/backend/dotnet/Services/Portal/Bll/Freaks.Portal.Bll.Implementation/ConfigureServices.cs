using Microsoft.Extensions.DependencyInjection;

namespace Freaks.Portal.Bll.Implementation;

/// <summary>
///     Расширения DI для Bll
/// </summary>
public static class ConfigureServices
{
    /// <summary>
    ///     Добавить Bll сервисы в DI
    /// </summary>
    /// <param name="services">Коллекция сервисов в DI</param>
    public static IServiceCollection AddBllServices(this IServiceCollection services)
    {
        return services;
    }
}