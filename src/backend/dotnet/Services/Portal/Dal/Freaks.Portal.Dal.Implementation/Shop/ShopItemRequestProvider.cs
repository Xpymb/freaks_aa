using Freaks.Dal.Common.Implementations;
using Freaks.Dal.Common.Interfaces;
using Freaks.Dal.Common.ValueObjects;
using Freaks.Portal.Contracts.Entities.Shop;
using Freaks.Portal.Dal.Interfaces.Shop;
using Freaks.Portal.Dal.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Freaks.Portal.Dal.Implementation.Shop;

/// <summary>
///     Провайдер для работы с заявками на покупку товаров из магазина.
///     Использует кэширование и предоставляет доступ к данным <see cref="ShopItemRequest" /> по составному ключу
///     <see cref="ShopItemRequestKey" />.
/// </summary>
public class ShopItemRequestProvider : BaseCachedCompositeProvider<ShopItemRequest, ShopItemRequestKey, IPortalDbContext>, IShopItemRequestProvider
{
    /// <summary>
    ///     Инициализирует новый экземпляр <see cref="ShopItemRequestProvider" />.
    /// </summary>
    /// <param name="dbContext">Контекст базы данных.</param>
    /// <param name="cacheProvider">Провайдер кэширования.</param>
    public ShopItemRequestProvider(IPortalDbContext dbContext, ICacheProvider cacheProvider) : base(dbContext, cacheProvider)
    {
    }

    /// <inheritdoc />
    public override async Task<ShopItemRequest?> GetAsync(ShopItemRequestKey key, EntityTrackingType trackingType)
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

        var query = FilterByKey(key, Set);

        if (trackingType is EntityTrackingType.NoTracking)
        {
            query = query.AsNoTracking();
        }

        var result =
            await query
                  .Include(r => r.User)
                  .FirstOrDefaultAsync();

        await SetCachedValueAsync(result, TimeSpan.FromMinutes(5));
        return result;
    }

    /// <inheritdoc />
    public async Task<IList<ShopItemRequest>> GetListAsync(int shopItemId)
    {
        var cacheKey = GetCacheShopItemKey(shopItemId);
        var cachedValue = await GetCachedValueAsync<IList<ShopItemRequest>>(cacheKey);
        if (cachedValue is not null)
        {
            return cachedValue;
        }

        var result =
            await Set
                  .Include(r => r.User)
                  .AsNoTracking()
                  .Where(r => r.ShopItemId == shopItemId)
                  .OrderBy(r => r.Status)
                  .ToListAsync();

        await SetCachedValueAsync(cacheKey, result, TimeSpan.FromMinutes(5));
        return result;
    }

    /// <inheritdoc />
    protected override IQueryable<ShopItemRequest> FilterByKey(ShopItemRequestKey key, IQueryable<ShopItemRequest> queryable)
    {
        return queryable.Where(r => (r.ShopItemId == key.ShopItemId) && (r.UserId == key.UserId));
    }

    /// <inheritdoc />
    protected override string GetCacheKey(ShopItemRequestKey key)
    {
        return $"{nameof(ShopItemRequest)}:shop-item:{key.ShopItemId}:user:{key.UserId}";
    }

    /// <inheritdoc />
    protected override List<string> GetAllCacheKeys(ShopItemRequest entity)
    {
        return
        [
            GetCacheKey(entity.GetCompositeKey()),
        ];
    }

    /// <inheritdoc />
    protected override List<string> GetAllCachePrefixes(ShopItemRequest entity)
    {
        return
        [
            GetDefaultCachePrefix(),
        ];
    }

    /// <summary>
    ///     Возвращает префикс кэша по умолчанию.
    /// </summary>
    private static string GetDefaultCachePrefix()
    {
        return $"{nameof(ShopItemRequest)}:list";
    }

    /// <summary>
    ///     Возвращает ключ кэша для списка заявок по идентификатору товара.
    /// </summary>
    /// <param name="shopItemId">Идентификатор товара.</param>
    /// <returns>Строка ключа кэша.</returns>
    private static string GetCacheShopItemKey(int shopItemId)
    {
        return $"{GetDefaultCachePrefix()}:shop-item:{shopItemId}";
    }
}