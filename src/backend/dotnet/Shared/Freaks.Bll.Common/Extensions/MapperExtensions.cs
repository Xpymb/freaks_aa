using Mapster;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;

namespace Freaks.Bll.Common.Extensions;

/// <summary>
///     Предоставляет расширение для регистрации общих настроек Mapster в контейнере DI.
/// </summary>
public static class MapperExtensions
{
    /// <summary>
    ///     Добавляет в сервисы глобальную конфигурацию Mapster с включённым маппингом через конструктор
    ///     и регистрирует <see cref="TypeAdapterConfig" /> и <see cref="IMapper" />.
    /// </summary>
    /// <param name="services">Коллекция сервисов для регистрации.</param>
    public static IServiceCollection AddMapsterCommon(this IServiceCollection services)
    {
        services.AddMapster();

        return services;
    }
}