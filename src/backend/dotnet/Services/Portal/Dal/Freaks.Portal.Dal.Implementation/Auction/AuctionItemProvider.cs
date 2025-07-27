using System.Text.Json;
using Freaks.Dal.Common.Extensions;
using Freaks.Dal.Common.Implementations;
using Freaks.Dal.Common.Interfaces;
using Freaks.Dal.Common.ValueObjects;
using Freaks.Portal.Contracts.Entities.Auction;
using Freaks.Portal.Dal.Interfaces.Auction;
using Freaks.Portal.Dal.Persistence;
using Freaks.Portal.SharedContracts.Requests.Auction.Item;
using Freaks.Portal.SharedContracts.ValueObjects.Auction;
using Freaks.SharedContracts.Common;
using Microsoft.EntityFrameworkCore;

namespace Freaks.Portal.Dal.Implementation.Auction;

/// <summary>
///     Провайдер кэшированных операций для работы с лотами аукциона.
/// </summary>
public class AuctionItemProvider : BaseCachedProvider<AuctionItem, long, IPortalDbContext>, IAuctionItemProvider
{
    /// <summary>
    ///     Инициализирует новый экземпляр <see cref="AuctionItemProvider" />,
    ///     устанавливая контекст портальной БД и провайдер кэша.
    /// </summary>
    /// <param name="dbContext">Экземпляр <see cref="IPortalDbContext" /> для работы с базой данных.</param>
    /// <param name="cacheProvider">Экземпляр <see cref="ICacheProvider" /> для управления кэшем.</param>
    public AuctionItemProvider(IPortalDbContext dbContext, ICacheProvider cacheProvider) : base(dbContext, cacheProvider)
    {
    }

    /// <inheritdoc />
    public override async Task<AuctionItem?> GetAsync(long key, EntityTrackingType trackingType)
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

        var result = await query.FirstOrDefaultAsync(i => i.Id == key);

        await SetCachedValueAsync(result, TimeSpan.FromMinutes(5));
        return result;
    }

    /// <inheritdoc />
    public async Task<PaginatedList<AuctionItem>> GetListAsync(GetAuctionItemListRequest request)
    {
        var cacheKey = GetParameterizedCacheKey(request);
        var cachedValue = await GetCachedValueAsync<PaginatedList<AuctionItem>>(cacheKey);
        if (cachedValue is not null)
        {
            return cachedValue;
        }

        var query =
            Set.Include(i => i.LootItem)
               .Include(i => i.Creator)
               .AsNoTracking();

        if (request.Statuses is not null
            && (request.Statuses.Count != 0))
        {
            query = query.Where(i => request.Statuses.Contains(i.Status));
        }

        if (request.From is not null)
        {
            query = query.Where(i => request.From >= i.CreatedDt);
        }

        if (request.To is not null)
        {
            query = query.Where(i => request.To <= i.CreatedDt);
        }

        query =
            request.SortBy switch
            {
                AuctionItemListSortByType.StartDt => query.OrderBy(i => i.CreatedDt, request.SortMode),
                AuctionItemListSortByType.EndDt => query.OrderBy(i => i.EndDt, request.SortMode),
                _ => throw new ArgumentOutOfRangeException(),
            };

        var result =
            await query.UseTakeSkip(request.Take, request.Skip)
                       .ToListAsync();

        var totalCount = await query.CountAsync();

        var paginatedResult = new PaginatedList<AuctionItem>(result, request.Take, request.Skip, totalCount);

        await SetCachedValueAsync(cacheKey, paginatedResult, TimeSpan.FromMinutes(5));
        return paginatedResult;
    }

    /// <inheritdoc />
    protected override string GetCacheKey(long key)
    {
        return $"{nameof(AuctionItem)}:{key}";
    }

    /// <inheritdoc />
    protected override List<string> GetAllCacheKeys(AuctionItem entity)
    {
        return
        [
            GetCacheKey(entity.Id),
        ];
    }

    /// <inheritdoc />
    protected override List<string> GetAllCachePrefixes(AuctionItem entity)
    {
        return
        [
            GetDefaultCachePrefix(),
        ];
    }

    /// <summary>
    ///     Общий префикс для ключей списка лотов аукциона.
    /// </summary>
    private static string GetDefaultCachePrefix()
    {
        return $"{nameof(AuctionItem)}:list";
    }

    /// <summary>
    ///     Формирует параметризованный ключ кэша для запроса списка по его параметрам.
    /// </summary>
    /// <param name="request">Параметры запроса списка лотов.</param>
    /// <returns>Строка-ключ вида "AuctionItem:list:{json}".</returns>
    private static string GetParameterizedCacheKey(GetAuctionItemListRequest request)
    {
        var json = JsonSerializer.Serialize(request);
        return $"{GetDefaultCachePrefix()}:{json}";
    }
}