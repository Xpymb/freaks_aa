using Centrifugo.AspNetCore.Configuration;
using Centrifugo.AspNetCore.Extensions;
using Freaks.Messages.Bll.Interfaces;
using Freaks.Messages.Centrifugo.Implementations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Freaks.Messages.Common;

/// <summary>
///     Расширения для регистрации сервиса публикации сообщений.
/// </summary>
public static class ConfigureServices
{
    /// <summary>
    ///     Регистрирует в контейнере DI клиент Centrifugo и сервис <see cref="IMessageService" />,
    ///     реализующий публикацию сообщений через Centrifugo.
    /// </summary>
    /// <param name="services">Коллекция сервисов для регистрации.</param>
    /// <param name="configuration">Конфигурация приложения для чтения параметров Centrifugo.</param>
    /// <returns>Ту же коллекцию <see cref="IServiceCollection" /> для цепочного вызова.</returns>
    public static IServiceCollection AddCentrifugoMessageService(this IServiceCollection services, IConfiguration configuration)
    {
        var centrifugoOptions =
            configuration.GetSection(nameof(CentrifugoOptions))
                         .Get<CentrifugoOptions>();

        services.AddCentrifugoClient(centrifugoOptions);

        services.AddScoped<IMessageService, CentrifugoMessageService>();

        return services;
    }
}