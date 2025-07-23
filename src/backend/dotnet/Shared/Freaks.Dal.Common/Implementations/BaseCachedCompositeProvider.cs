using Freaks.Contracts.Common.Interfaces;
using Freaks.Dal.Common.Interfaces;
using Freaks.Dal.Common.ValueObjects;

namespace Freaks.Dal.Common.Implementations;

/// <summary>
///     Абстрактный провайдер, реализующий кэширование сущностей с составным ключом
///     в дополнение к базовой логике доступа к данным.
/// </summary>
/// <typeparam name="TEntity">Тип сущности, реализующий <see cref="ICompositeEntity{TKey}" />.</typeparam>
/// <typeparam name="TKey">Тип составного ключа (обычно record).</typeparam>
/// <typeparam name="TDbContext">Тип контекста базы данных.</typeparam>
public abstract class BaseCachedCompositeProvider<TEntity, TKey, TDbContext> : BaseCompositeProvider<TEntity, TKey, TDbContext>
    where TEntity : class, ICompositeEntity<TKey>
    where TDbContext : IBaseDbContext
{
    private readonly ICacheProvider _cacheProvider;

    /// <summary>
    ///     Инициализирует новый экземпляр <see cref="BaseCachedCompositeProvider{TEntity,TKey,TDbContext}" />.
    /// </summary>
    /// <param name="dbContext">Контекст базы данных.</param>
    /// <param name="cacheProvider">Провайдер кэша.</param>
    protected BaseCachedCompositeProvider(TDbContext dbContext, ICacheProvider cacheProvider)
        : base(dbContext)
    {
        _cacheProvider = cacheProvider ?? throw new ArgumentNullException(nameof(cacheProvider));
    }

    /// <inheritdoc />
    public override async Task<TEntity?> GetAsync(TKey key, EntityTrackingType trackingType)
    {
        var cachedValue = await GetCachedValueAsync(key);
        if (cachedValue is not null)
        {
            if (trackingType is EntityTrackingType.Tracking)
            {
                Set.Attach(cachedValue);
            }

            return cachedValue;
        }

        var result = await base.GetAsync(key, trackingType);
        await SetCachedValueAsync(key, result, TimeSpan.FromMinutes(5));
        return result;
    }

    /// <inheritdoc />
    public override async Task<TEntity> CreateAsync(TEntity entity)
    {
        await RemoveCacheAsync(entity);
        return await base.CreateAsync(entity);
    }

    /// <inheritdoc />
    public override async Task<IList<TEntity>> SetAsync(IList<TEntity> entities)
    {
        await RemoveCacheAsync(entities);
        return await base.SetAsync(entities);
    }

    /// <inheritdoc />
    public override async Task<TEntity> UpdateAsync(TEntity entity)
    {
        await RemoveCacheAsync(entity);
        return await base.UpdateAsync(entity);
    }

    /// <inheritdoc />
    public override async Task<IList<TEntity>> UpdateSetAsync(IList<TEntity> entities)
    {
        await RemoveCacheAsync(entities);
        return await base.UpdateSetAsync(entities);
    }

    /// <inheritdoc />
    public override async Task DeleteAsync(TKey key)
    {
        await RemoveCacheAsync(key);
        await base.DeleteAsync(key);
    }

    /// <inheritdoc />
    public override async Task DeleteAsync(TEntity entity)
    {
        await RemoveCacheAsync(entity);
        await base.DeleteAsync(entity);
    }

    /// <inheritdoc />
    public override async Task DeleteAsync(IList<TKey> keys)
    {
        await RemoveCacheAsync(keys);
        await base.DeleteAsync(keys);
    }

    /// <inheritdoc />
    public override async Task DeleteAsync(IList<TEntity> entities)
    {
        await RemoveCacheAsync(entities);
        await base.DeleteAsync(entities);
    }

    /// <summary>
    ///     Возвращает строковой ключ для кэша по составному ключу сущности.
    /// </summary>
    /// <param name="key">Составной ключ сущности.</param>
    /// <returns>Строка — ключ кэша.</returns>
    protected abstract string GetCacheKey(TKey key);

    /// <summary>
    ///     Возвращает список всех возможных ключей кэша, связанных с объектом.
    /// </summary>
    /// <param name="entity">Сущность.</param>
    /// <returns>Список ключей кэша.</returns>
    protected abstract List<string> GetAllCacheKeys(TEntity entity);

    /// <summary>
    ///     Возвращает список всех префиксов кэша, связанных с объектом.
    /// </summary>
    /// <param name="entity">Сущность.</param>
    /// <returns>Список префиксов.</returns>
    protected abstract List<string> GetAllCachePrefixes(TEntity entity);

    protected async Task<TEntity?> GetCachedValueAsync(TKey key)
    {
        var cacheKey = GetCacheKey(key);
        return await _cacheProvider.GetAsync<TEntity>(cacheKey);
    }

    protected async Task<TResponse?> GetCachedValueAsync<TResponse>(string cacheKey)
    {
        return await _cacheProvider.GetAsync<TResponse>(cacheKey);
    }

    protected async Task SetCachedValueAsync(TKey key, TEntity? entity, TimeSpan expiration)
    {
        if (entity is null)
        {
            return;
        }

        var cacheKey = GetCacheKey(key);
        await _cacheProvider.SetAsync(cacheKey, entity, expiration);
    }

    protected async Task SetCachedValueAsync<T>(string cacheKey, T? entity, TimeSpan expiration)
    {
        if (entity is null)
        {
            return;
        }

        await _cacheProvider.SetAsync(cacheKey, entity, expiration);
    }

    protected async Task RemoveCacheAsync(TEntity entity)
    {
        var keys = GetAllCacheKeys(entity);
        var prefixes = GetAllCachePrefixes(entity);
        await _cacheProvider.RemoveAsync(keys);
        await _cacheProvider.RemoveByPrefixAsync(prefixes);
    }

    protected async Task RemoveCacheAsync(TKey key)
    {
        var entity = await GetAsync(key, EntityTrackingType.NoTracking);
        if (entity is null)
        {
            return;
        }

        await RemoveCacheAsync(entity);
    }

    protected async Task RemoveCacheAsync(IList<TKey> keys)
    {
        foreach (var key in keys)
        {
            await RemoveCacheAsync(key);
        }
    }

    protected async Task RemoveCacheAsync(IList<TEntity> entities)
    {
        foreach (var entity in entities)
        {
            await RemoveCacheAsync(entity);
        }
    }
}