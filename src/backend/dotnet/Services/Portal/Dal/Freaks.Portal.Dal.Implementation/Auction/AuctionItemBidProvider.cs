using Freaks.Dal.Common.Implementations;
using Freaks.Dal.Common.Interfaces;
using Freaks.Portal.Contracts.Entities.Auction;
using Freaks.Portal.Dal.Interfaces.Auction;
using Freaks.Portal.Dal.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Freaks.Portal.Dal.Implementation.Auction;

/// <summary>
///     Провайдер кэшированных операций для работы со ставками по лотам аукциона.
/// </summary>
public class AuctionItemBidProvider : BaseCachedProvider<AuctionItemBid, long, IPortalDbContext>, IAuctionItemBidProvider
{
    /// <summary>
    ///     Инициализирует новый экземпляр <see cref="AuctionItemBidProvider" />,
    ///     устанавливая контекст портальной БД и провайдер кэша.
    /// </summary>
    /// <param name="dbContext">Экземпляр <see cref="IPortalDbContext" /> для работы с базой данных.</param>
    /// <param name="cacheProvider">Экземпляр <see cref="ICacheProvider" /> для управления кэшем.</param>
    public AuctionItemBidProvider(IPortalDbContext dbContext, ICacheProvider cacheProvider) : base(dbContext, cacheProvider)
    {
    }

    /// <inheritdoc />
    public async Task<IList<AuctionItemBid>> GetListAsync(long auctionItemId)
    {
        var cacheKey = GetCacheAuctionItemKey(auctionItemId);
        var cachedValue = await GetCachedValueAsync<IList<AuctionItemBid>>(cacheKey);
        if (cachedValue is not null)
        {
            return cachedValue;
        }

        var result =
            await Set.Include(b => b.Creator)
                     .AsNoTracking()
                     .Where(b => b.AuctionItemId == auctionItemId)
                     .OrderByDescending(b => b.Price)
                     .ToListAsync();

        await SetCachedValueAsync(cacheKey, result, TimeSpan.FromMinutes(5));
        return result;
    }

    /// <inheritdoc />
    public async Task<AuctionItemBid?> GetLastAsync(long auctionItemId)
    {
        var cacheKey = GetCacheLastKey(auctionItemId);
        var cachedValue = await GetCachedValueAsync<AuctionItemBid>(cacheKey);
        if (cachedValue is not null)
        {
            return cachedValue;
        }

        var result =
            await Set.Include(b => b.Creator)
                     .AsNoTracking()
                     .Where(b => b.AuctionItemId == auctionItemId)
                     .OrderByDescending(b => b.Price)
                     .FirstOrDefaultAsync();

        await SetCachedValueAsync(cacheKey, result, TimeSpan.FromMinutes(5));
        return result;
    }

    /// <inheritdoc />
    protected override string GetCacheKey(long key)
    {
        return $"{nameof(AuctionItemBid)}:{key}";
    }

    /// <inheritdoc />
    protected override List<string> GetAllCacheKeys(AuctionItemBid entity)
    {
        return
        [
            GetCacheKey(entity.Id),
            GetCacheLastKey(entity.AuctionItemId),
        ];
    }

    /// <inheritdoc />
    protected override List<string> GetAllCachePrefixes(AuctionItemBid entity)
    {
        return
        [
            GetDefaultCachePrefix(),
        ];
    }

    /// <summary>
    ///     Возвращает префикс кэша для списка ставок по всем лотам аукциона.
    /// </summary>
    /// <returns>Строка-ключ префикса вида "AuctionItemBid:list".</returns>
    private static string GetDefaultCachePrefix()
    {
        return $"{nameof(AuctionItemBid)}:list";
    }

    /// <summary>
    ///     Формирует параметризованный ключ кэша для списка ставок по идентификатору лота.
    /// </summary>
    /// <param name="auctionItemId">Идентификатор лота.</param>
    /// <returns>Строка-ключ вида "AuctionItemBid:list:auction_item_id:{auctionItemId}".</returns>
    private static string GetCacheAuctionItemKey(long auctionItemId)
    {
        return $"{GetDefaultCachePrefix()}:auction_item_id:{auctionItemId}";
    }

    /// <summary>
    ///     Формирует ключ кэша для получения последней ставки по указанному лоту.
    /// </summary>
    /// <param name="auctionItemId">Идентификатор лота, для которого формируется ключ.</param>
    /// <returns>Строка-ключ вида "AuctionItemBid:last:auction_item_id:{auctionItemId}".</returns>
    private static string GetCacheLastKey(long auctionItemId)
    {
        return $"{nameof(AuctionItemBid)}:last:auction_item_id:{auctionItemId}";
    }
}