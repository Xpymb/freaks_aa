using System.Text.Json;
using Freaks.Dal.Common.Extensions;
using Freaks.Dal.Common.Implementations;
using Freaks.Dal.Common.Interfaces;
using Freaks.Dal.Common.ValueObjects;
using Freaks.Portal.Contracts.Entities.Shop;
using Freaks.Portal.Dal.Interfaces.Shop;
using Freaks.Portal.Dal.Persistence;
using Freaks.Portal.SharedContracts.Requests.Shop.ShopItem;
using Freaks.SharedContracts.Common;
using Microsoft.EntityFrameworkCore;

namespace Freaks.Portal.Dal.Implementation.Shop;

/// <summary>
///     Провайдер для работы с товарами магазина с поддержкой кэширования.
///     Предоставляет возможность получения списка товаров с фильтрацией, пагинацией и кэшированием.
/// </summary>
public class ShopItemProvider : BaseCachedProvider<ShopItem, int, IPortalDbContext>, IShopItemProvider
{
    /// <summary>
    ///     Инициализирует новый экземпляр <see cref="ShopItemProvider" />.
    /// </summary>
    /// <param name="dbContext">Контекст базы данных портала.</param>
    /// <param name="cacheProvider">Провайдер кэширования.</param>
    public ShopItemProvider(IPortalDbContext dbContext, ICacheProvider cacheProvider) : base(dbContext, cacheProvider)
    {
    }

    /// <inheritdoc />
    public override async Task<ShopItem?> GetAsync(int key, EntityTrackingType trackingType)
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

        var query =
            Set.Include(i => i.LootItem)
               .Include(i => i.Creator)
               .AsQueryable();

        if (trackingType is EntityTrackingType.NoTracking)
        {
            query = query.AsNoTracking();
        }

        var result =
            await query
                .FirstOrDefaultAsync(i => i.Id == key);

        await SetCachedValueAsync(result, TimeSpan.FromMinutes(5));
        return result;
    }

    /// <inheritdoc />
    public async Task<PaginatedList<ShopItem>> GetListAsync(GetShopItemListRequest request)
    {
        var cacheKey = GetParameterizedCacheKey(request);
        var cachedValue = await GetCachedValueAsync<PaginatedList<ShopItem>>(cacheKey);
        if (cachedValue is not null)
        {
            return cachedValue;
        }

        var query =
            Set
                .Include(i => i.LootItem)
                .Include(i => i.Creator)
                .AsNoTracking();

        if (request.Status is not null)
        {
            query = query.Where(x => x.Status == request.Status);
        }

        var result =
            await query.UseTakeSkip(request.Take, request.Skip)
                       .ToListAsync();

        var totalCount = await query.CountAsync();

        var paginatedResult = new PaginatedList<ShopItem>(result, request.Take, request.Skip, totalCount);

        await SetCachedValueAsync(cacheKey, paginatedResult, TimeSpan.FromMinutes(5));
        return paginatedResult;
    }

    /// <inheritdoc />
    protected override string GetCacheKey(int key)
    {
        return $"{nameof(ShopItem)}:{key}";
    }

    /// <inheritdoc />
    protected override List<string> GetAllCacheKeys(ShopItem entity)
    {
        return
        [
            GetCacheKey(entity.Id),
        ];
    }

    /// <inheritdoc />
    protected override List<string> GetAllCachePrefixes(ShopItem entity)
    {
        return
        [
            GetDefaultCachePrefix(),
        ];
    }

    /// <summary>
    ///     Возвращает префикс по умолчанию для кэширования списка товаров магазина.
    /// </summary>
    /// <returns>Строковый префикс кэша.</returns>
    private static string GetDefaultCachePrefix()
    {
        return $"{nameof(ShopItem)}:list";
    }

    /// <summary>
    ///     Генерирует параметризованный кэш-ключ на основе фильтра запроса.
    ///     Используется для кэширования результатов с учётом параметров фильтрации и пагинации.
    /// </summary>
    /// <param name="request">Запрос с параметрами фильтрации.</param>
    /// <returns>Уникальный строковый ключ кэша.</returns>
    private static string GetParameterizedCacheKey(GetShopItemListRequest request)
    {
        var jsonParameters = JsonSerializer.Serialize(request);
        return $"{GetDefaultCachePrefix()}:{jsonParameters}";
    }
}