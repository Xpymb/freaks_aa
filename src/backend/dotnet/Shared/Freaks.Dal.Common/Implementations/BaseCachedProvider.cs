using Freaks.Contracts.Common.Interfaces;
using Freaks.Dal.Common.Interfaces;
using Freaks.Dal.Common.ValueObjects;

namespace Freaks.Dal.Common.Implementations;

/// <summary>
///     Абстрактный провайдер, реализующий кэширование сущностей в дополнение к базовой логике доступа к данным.
/// </summary>
/// <typeparam name="TEntity">Тип сущности.</typeparam>
/// <typeparam name="TKey">Тип ключа сущности.</typeparam>
/// <typeparam name="TDbContext">Тип контекста базы данных.</typeparam>
public abstract class BaseCachedProvider<TEntity, TKey, TDbContext> : BaseProvider<TEntity, TKey, TDbContext>
    where TEntity : class, IEntity<TKey>
    where TKey : IEquatable<TKey>
    where TDbContext : IBaseDbContext
{
    private readonly ICacheProvider _cacheProvider;

    /// <summary>
    ///     Инициализирует новый экземпляр <see cref="BaseCachedProvider{TEntity,TKey,TDbContext}" />.
    /// </summary>
    /// <param name="dbContext">Контекст базы данных.</param>
    /// <param name="cacheProvider">Провайдер кэша.</param>
    protected BaseCachedProvider(TDbContext dbContext, ICacheProvider cacheProvider) : base(dbContext)
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

        await SetCachedValueAsync(result, TimeSpan.FromMinutes(5));
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
        var result = await base.UpdateAsync(entity);
        await RemoveCacheAsync(entity);
        return result;
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
    ///     Получает уникальный ключ кэша по значению ключа сущности.
    /// </summary>
    /// <param name="key">Ключ сущности.</param>
    /// <returns>Строка — ключ для кэша.</returns>
    protected abstract string GetCacheKey(TKey key);

    /// <summary>
    ///     Возвращает список всех возможных ключей кэша, связанных с объектом.
    /// </summary>
    /// <param name="entity">Сущность.</param>
    /// <returns>Список ключей кэша.</returns>
    protected abstract List<string> GetAllCacheKeys(TEntity entity);

    /// <summary>
    ///     Возвращает список всех префиксов ключей кэша, связанных с объектом.
    /// </summary>
    /// <param name="entity">Сущность.</param>
    /// <returns>Список префиксов ключей.</returns>
    protected abstract List<string> GetAllCachePrefixes(TEntity entity);

    /// <summary>
    ///     Получает закэшированное значение сущности по ключу.
    /// </summary>
    /// <param name="key">Ключ сущности.</param>
    /// <returns>Сущность или null.</returns>
    protected async Task<TEntity?> GetCachedValueAsync(TKey key)
    {
        var cacheKey = GetCacheKey(key);
        return await _cacheProvider.GetAsync<TEntity>(cacheKey);
    }

    /// <summary>
    ///     Получает закэшированное значение по заданному ключу и типу.
    /// </summary>
    /// <typeparam name="TResponse">Тип ожидаемого значения.</typeparam>
    /// <param name="cacheKey">Ключ кэша.</param>
    /// <returns>Объект из кэша или null.</returns>
    protected async Task<TResponse?> GetCachedValueAsync<TResponse>(string cacheKey)
    {
        return await _cacheProvider.GetAsync<TResponse>(cacheKey);
    }

    /// <summary>
    ///     Устанавливает значение сущности в кэш по ключу и времени хранения.
    /// </summary>
    /// <param name="key">Ключ сущности.</param>
    /// <param name="entity">Сущность.</param>
    /// <param name="expiration">Время хранения в кэше.</param>
    protected async Task SetCachedValueAsync(TEntity? entity, TimeSpan expiration)
    {
        if (entity is null)
        {
            return;
        }

        var cacheKey = GetCacheKey(entity.Id);
        await _cacheProvider.SetAsync(cacheKey, entity, expiration);
    }

    /// <summary>
    ///     Устанавливает значение сущности в кэш по строковому ключу.
    /// </summary>
    /// <typeparam name="T">Тип сохраняемого значения.</typeparam>
    /// <param name="cacheKey">Ключ кэша.</param>
    /// <param name="entity">Сущность.</param>
    /// <param name="expiration">Время хранения.</param>
    protected async Task SetCachedValueAsync<T>(string cacheKey, T? entity, TimeSpan expiration)
    {
        if (entity is null)
        {
            return;
        }

        await _cacheProvider.SetAsync(cacheKey, entity, expiration);
    }

    /// <summary>
    ///     Удаляет все связанные записи из кэша для переданной сущности.
    /// </summary>
    /// <param name="entity">Сущность.</param>
    protected async Task RemoveCacheAsync(TEntity entity)
    {
        var allCacheKeys = GetAllCacheKeys(entity);
        var allCachePrefixes = GetAllCachePrefixes(entity);

        await _cacheProvider.RemoveAsync(allCacheKeys);
        await _cacheProvider.RemoveByPrefixAsync(allCachePrefixes);
    }

    /// <summary>
    ///     Удаляет записи из кэша по ключу сущности.
    /// </summary>
    /// <param name="key">Ключ сущности.</param>
    protected async Task RemoveCacheAsync(TKey key)
    {
        var entity = await GetAsync(key, EntityTrackingType.NoTracking);
        if (entity is null)
        {
            return;
        }

        await RemoveCacheAsync(entity);
    }

    /// <summary>
    ///     Удаляет записи из кэша по списку ключей.
    /// </summary>
    /// <param name="keys">Список ключей.</param>
    protected async Task RemoveCacheAsync(IList<TKey> keys)
    {
        foreach (var key in keys)
        {
            var entity = await GetAsync(key, EntityTrackingType.NoTracking);
            if (entity is null)
            {
                return;
            }

            await RemoveCacheAsync(entity);
        }
    }

    /// <summary>
    ///     Удаляет записи из кэша для списка сущностей.
    /// </summary>
    /// <param name="entities">Список сущностей.</param>
    protected async Task RemoveCacheAsync(IList<TEntity> entities)
    {
        foreach (var entity in entities)
        {
            await RemoveCacheAsync(entity);
        }
    }
}