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
    public static IServiceCollection AddEasyCachingCommon(this IServiceCollection services, IConfiguration configuration)
    {
        var redisOptionsSection = configuration.GetSection(nameof(RedisOptions));

        services.Configure<RedisOptions>(redisOptionsSection);
        var redisOptions = redisOptionsSection.Get<RedisOptions>();

        services.AddLZ4Compressor();

        services.AddEasyCaching(options =>
        {
            options.UseRedis(
                config =>
                {
                    config.DBConfig.Endpoints.Add(new ServerEndPoint(redisOptions!.Host, redisOptions.Port!.Value));
                    config.DBConfig.Password = redisOptions.Password;
                    config.DBConfig.Database = redisOptions.Database!.Value;
                    config.DBConfig.ConnectionTimeout = redisOptions.ConnectTimeout;
                    config.DBConfig.SyncTimeout = redisOptions.SyncTimeout;
                    config.DBConfig.AllowAdmin = true;
                    config.DBConfig.AbortOnConnectFail = false;

                    config.SerializerName = "msgpack";
                },
                "redis")
                   .WithMessagePack()
                   .WithCompressor();

            // options.UseInMemory("in_memory");
            //
            // options.WithRedisBus(bus =>
            // {
            //     bus.Endpoints.Add(new ServerEndPoint(redis!.Host, redis.Port!.Value));
            //     bus.Password = redis.Password;
            // });
            //
            // options.UseHybrid(
            //     config =>
            //     {
            //         config.TopicName = "freaks_hybrid_cache_channel";
            //         config.LocalCacheProviderName = "in_memory";
            //         config.DistributedCacheProviderName = "redis";
            //     },
            //     "hybrid");
        });

        services.AddScoped<ICacheProvider, EasyCacheProvider>();

        return services;
    }
}