using Freaks.Dal.Common.Implementations;
using Freaks.Dal.Common.Interfaces;
using Freaks.Options.Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Freaks.Dal.Common.Extensions;

/// <summary>
/// Расширения для регистрации PostgreSQL-контекста данных в DI-контейнер
/// и автоматического применения миграций.
/// </summary>
public static class DbExtensions
{
    /// <summary>
    /// Регистрирует <see cref="DbContext" /> на основе PostgreSQL.
    /// Также добавляет реализацию контекста как scoped-сервис по контракту интерфейса.
    /// </summary>
    /// <typeparam name="TInterface">
    /// Интерфейс контекста, например <c>IAppDbContext</c>. Должен реализовывать <see cref="IBaseDbContext"/>.
    /// </typeparam>
    /// <typeparam name="TService">
    /// Класс, реализующий DbContext, например <c>AppDbContext</c>. Должен наследовать <see cref="DbContext" /> и реализовывать <typeparamref name="TInterface"/>.
    /// </typeparam>
    /// <param name="services">Коллекция сервисов приложения.</param>
    /// <param name="configuration">Конфигурация приложения для получения строки подключения.</param>
    /// <returns>Обновлённая коллекция сервисов.</returns>
    public static IServiceCollection AddPostgresDbContext<TInterface, TService>(this IServiceCollection services, IConfiguration configuration)
        where TService : DbContext, TInterface
        where TInterface : class, IBaseDbContext
    {
        var dbOptionsSection = configuration.GetSection(nameof(DbOptions));

        services.Configure<DbOptions>(dbOptionsSection);
        var dbOptions = dbOptionsSection.Get<DbOptions>();

        services.AddDbContext<TService>(options => options.UseNpgsql(dbOptions?.ConnectionString ?? throw new NullReferenceException()));
        services.AddScoped<TInterface>(provider => provider.GetRequiredService<TService>());

        services.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));

        return services;
    }

    /// <summary>
    /// Применяет все ожидающие миграции для указанного контекста базы данных при запуске приложения.
    /// </summary>
    /// <typeparam name="TDbContext">Тип контекста, реализующий <see cref="IBaseDbContext"/>.</typeparam>
    /// <param name="app">Экземпляр приложения <see cref="IApplicationBuilder"/>.</param>
    /// <returns>Обновлённый экземпляр <see cref="IApplicationBuilder"/>.</returns>
    public static async Task<IApplicationBuilder> ApplyMigrationsAsync<TDbContext>(this IApplicationBuilder app)
        where TDbContext : IBaseDbContext
    {
        using var scope = app.ApplicationServices.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<TDbContext>();
        await dbContext.Database.MigrateAsync();

        return app;
    }
}