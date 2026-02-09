using System.Reflection;
using FastExpressionCompiler;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

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
    /// <param name="assembly">Сборка приложения, в которой находятся конфиги</param>
    public static IServiceCollection AddMapsterCommon(this IServiceCollection services, Assembly assembly)
    {
        var config = TypeAdapterConfig.GlobalSettings;

        config.Compiler = exp => exp.CompileFast();
        config.Scan(assembly);

        services.AddSingleton(config);
        services.TryAddScoped<IMapper, ServiceMapper>();

        return services;
    }
}