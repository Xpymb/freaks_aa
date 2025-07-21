using EasyCaching.Core.Configurations;
using Freaks.Dal.Common.Implementations;
using Freaks.Dal.Common.Interfaces;
using Freaks.Options.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Freaks.Dal.Common.Extensions;

/// <summary>
///     Расширения для настройки Redis, InMemory и гибридным кэшем.
/// </summary>
public static class CacheExtensions
{
    /// <summary>
    ///     Регистрирует EasyCaching с поддержкой Redis, InMemory и гибридного провайдера.
    ///     Также настраивает сериализацию и компрессию, а также Pub/Sub канал синхронизации.
    /// </summary>
    /// <param name="services">Коллекция сервисов DI.</param>
    /// <param name="configuration">Конфигурация приложения (для получения настроек Redis).</param>
    /// <returns>Обновлённая коллекция сервисов.</returns>
    public static IServiceCollection AddEasyCaching(this IServiceCollection services, IConfiguration configuration)
    {
        var redis =
            configuration.GetSection("RedisOptions")
                         .Get<RedisOptions>();

        services.AddEasyCaching(options =>
        {
            options.UseRedis(
                config =>
                {
                    config.DBConfig.Endpoints.Add(new ServerEndPoint(redis!.Host, redis.Port!.Value));
                    config.DBConfig.Password = redis.Password;
                    config.DBConfig.Database = 0;
                },
                "redis");

            options.UseInMemory("in_memory");

            options.UseHybrid(
                config =>
                {
                    config.TopicName = "freaks_hybrid_cache_channel";
                    config.LocalCacheProviderName = "in_memory";
                    config.DistributedCacheProviderName = "redis";
                },
                "hybrid");

            options.WithRedisBus(bus =>
            {
                bus.Endpoints.Add(new ServerEndPoint(redis!.Host, redis.Port!.Value));
                bus.Password = redis.Password;
            });

            options
                .WithMessagePack()
                .WithCompressor();
        });

        services.AddScoped<ICacheProvider, EasyCacheProvider>();

        return services;
    }
}