using EasyCaching.Core;
using Freaks.Dal.Common.Interfaces;

namespace Freaks.Dal.Common.Implementations;

/// <summary>
///     Реализация интерфейса <see cref="ICacheProvider" /> с использованием EasyCaching.
/// </summary>
public class EasyCacheProvider : ICacheProvider
{
    private readonly IEasyCachingProvider _provider;

    /// <summary>
    ///     Создаёт новый экземпляр <see cref="EasyCacheProvider" /> с использованием фабрики кэш-провайдеров.
    /// </summary>
    /// <param name="factory">Фабрика для получения провайдера кэша.</param>
    public EasyCacheProvider(IEasyCachingProviderFactory factory)
    {
        // _provider = factory.GetCachingProvider("hybrid");
        _provider = factory.GetCachingProvider("redis");
    }

    /// <inheritdoc />
    public async Task<T?> GetAsync<T>(string key)
    {
        var result = await _provider.GetAsync<T>(key);
        return result.HasValue ? result.Value : default;
    }

    /// <inheritdoc />
    public Task SetAsync<T>(string key, T value, TimeSpan expiration)
    {
        return _provider.SetAsync(key, value, expiration);
    }

    /// <inheritdoc />
    public Task RemoveAsync(string key)
    {
        return _provider.RemoveAsync(key);
    }

    /// <inheritdoc />
    public Task RemoveAsync(List<string> keys)
    {
        return _provider.RemoveAllAsync(keys);
    }

    /// <inheritdoc />
    public Task RemoveByPrefixAsync(string prefix)
    {
        return _provider.RemoveByPrefixAsync(prefix);
    }

    /// <inheritdoc />
    public async Task RemoveByPrefixAsync(List<string> prefixes)
    {
        foreach (var prefix in prefixes)
        {
            await _provider.RemoveByPrefixAsync(prefix);
        }
    }
}