using Freaks.Dal.Common.Implementations;
using Freaks.Dal.Common.Interfaces;
using Freaks.Dal.Common.ValueObjects;
using Freaks.Portal.Contracts.Entities.RaidSummary;
using Freaks.Portal.Dal.Interfaces.RaidSummary;
using Freaks.Portal.Dal.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Freaks.Portal.Dal.Implementation.RaidSummary;

/// <summary>
/// Провайдер доступа к данным о луте, полученном в рейдах.
/// Обеспечивает взаимодействие с базой данных и кэшированием для операций получения, создания, обновления и удаления лута.
/// </summary>
public class RaidLootProvider : BaseCachedCompositeProvider<RaidLoot, RaidLootKey, IPortalDbContext>, IRaidLootProvider
{
    /// <summary>
    /// Инициализирует новый экземпляр <see cref="RaidLootProvider"/>.
    /// </summary>
    /// <param name="cacheProvider">Провайдер кэширования.</param>
    /// <param name="portalDbContext">Контекст базы данных портала.</param>
    public RaidLootProvider(
        ICacheProvider cacheProvider,
        IPortalDbContext portalDbContext) : base(portalDbContext, cacheProvider)
    {
    }

    /// <inheritdoc />
    public override async Task<RaidLoot?> GetAsync(RaidLootKey key, EntityTrackingType trackingType)
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
                  .Include(l => l.LootItemId)
                  .FirstOrDefaultAsync();

        await SetCachedValueAsync(result, TimeSpan.FromMinutes(5));
        return result;
    }

    /// <inheritdoc />
    public async Task<IList<RaidLoot>> GetByRaidIdAsync(long raidId)
    {
        var cacheKey = GetCacheRaidKey(raidId);
        var cachedValue = await GetCachedValueAsync<IList<RaidLoot>>(cacheKey);
        if (cachedValue is not null)
        {
            return cachedValue;
        }

        var result =
            await Set
                  .Include(l => l.Loot)
                  .AsNoTracking()
                  .Where(l => l.RaidId == raidId)
                  .ToListAsync();

        await SetCachedValueAsync(cacheKey, result, TimeSpan.FromMinutes(5));
        return result;
    }
    
    /// <inheritdoc />
    protected override IQueryable<RaidLoot> FilterByKey(RaidLootKey key, IQueryable<RaidLoot> queryable)
    {
        return queryable.Where(l => (l.RaidId == key.RaidId) && (l.LootItemId == key.LootId));
    }

    protected override string GetCacheKey(RaidLootKey key)
    {
        return $"{nameof(RaidLoot)}:raid:{key.RaidId}:loot:{key.LootId}";
    }

    protected override List<string> GetAllCacheKeys(RaidLoot entity)
    {
        return
        [
            GetCacheKey(entity.GetCompositeKey()),
            GetCacheRaidKey(entity.RaidId),
        ];
    }

    protected override List<string> GetAllCachePrefixes(RaidLoot entity)
    {
        return [];
    }
    
    /// <summary>
    /// Возвращает ключ кэша для списка лута, связанного с рейдом.
    /// </summary>
    /// <param name="raidId">Идентификатор рейда.</param>
    /// <returns>Ключ кэша.</returns>
    private static string GetCacheRaidKey(long raidId)
    {
        return $"{nameof(RaidLoot)}:list:raid:{raidId}";
    }
}