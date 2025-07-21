using Freaks.Dal.Common.Interfaces;
using Freaks.Options.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Freaks.Dal.Common.Extensions;

/// <summary>
///     Расширения для регистрации PostgreSQL-контекста данных в DI-контейнер.
/// </summary>
public static class DbExtensions
{
    /// <summary>
    ///     Регистрирует DbContext на основе PostgreSQL.
    ///     Также регистрирует интерфейсный контракт контекста как scoped-сервис.
    /// </summary>
    /// <typeparam name="TInterface">Интерфейс контекста (например, <c>IAppDbContext</c>).</typeparam>
    /// <typeparam name="TService">Реализация DbContext (например, <c>AppDbContext</c>).</typeparam>
    /// <param name="services">Коллекция сервисов приложения.</param>
    /// <param name="configuration">Конфигурация приложения (например, <c>IConfiguration</c>).</param>
    public static IServiceCollection AddPostgresDbContext<TInterface, TService>(this IServiceCollection services, IConfiguration configuration)
        where TService : DbContext, TInterface
        where TInterface : class, IBaseDbContext
    {
        var dbOptions =
            configuration
                .GetSection(nameof(DbOptions))
                .Get<DbOptions>();

        services.AddDbContext<TService>(options => options.UseNpgsql(dbOptions?.ConnectionString ?? throw new NullReferenceException()));
        services.AddScoped<TInterface>(provider => provider.GetRequiredService<TService>());

        return services;
    }
}