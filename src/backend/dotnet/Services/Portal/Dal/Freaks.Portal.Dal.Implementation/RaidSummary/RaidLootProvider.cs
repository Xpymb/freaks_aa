using Freaks.Dal.Common.Interfaces;
using Freaks.Portal.Contracts.Entities.RaidSummary;
using Freaks.Portal.Dal.Interfaces.RaidSummary;
using Freaks.Portal.Dal.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Freaks.Portal.Dal.Implementation.RaidSummary;

public class RaidLootProvider : IRaidLootProvider
{
    private readonly ICacheProvider _cacheProvider;
    private readonly IPortalDbContext _dbContext;

    public RaidLootProvider(
        ICacheProvider cacheProvider,
        IPortalDbContext portalDbContext)
    {
        _cacheProvider = cacheProvider ?? throw new ArgumentNullException(nameof(cacheProvider));
        _dbContext = portalDbContext ?? throw new ArgumentNullException(nameof(portalDbContext));
    }

    /// <inheritdoc />
    public async Task<IList<RaidLoot>> GetByRaidIdAsync(int raidId)
    {
        var cacheKey = GetCacheRaidKey(raidId);
        var cachedValue = await _cacheProvider.GetAsync<IList<RaidLoot>>(cacheKey);
        if (cachedValue is not null)
        {
            return cachedValue;
        }
        
        var result = await _dbContext.RaidLoots
                                     .Include(l => l.Loot)
                                     .AsNoTracking()
                                     .Where(l => l.RaidId == raidId)
                                     .ToListAsync();
        
        await _cacheProvider.SetAsync(cacheKey, result, TimeSpan.FromMinutes(5));
        return result;
    }

    /// <inheritdoc />
    public async Task<RaidLoot> CreateAsync(RaidLoot loot)
    {
        await RemoveCacheAsync(loot);
        
        var entry = await _dbContext.RaidLoots.AddAsync(loot);
        await _dbContext.SaveChangesAsync();
        
        return entry.Entity;
    }

    /// <inheritdoc />
    public async Task DeleteAsync(RaidLoot loot)
    {
        await RemoveCacheAsync(loot);

        await _dbContext.RaidLoots
                        .Where(l => l.RaidId == loot.RaidId && l.LootId != loot.LootId)
                        .ExecuteDeleteAsync();
    }

    private async Task RemoveCacheAsync(RaidLoot loot)
    {
        var cacheKeys = GetAllCacheKeys(loot);
        await _cacheProvider.RemoveAsync(cacheKeys);
    }
    
    private static string GetCacheRaidKey(int raidId)
    {
        return $"{nameof(RaidLoot)}:list:raid:{raidId}";
    }

    private static List<string> GetAllCacheKeys(RaidLoot loot)
    {
        return
        [
            GetCacheRaidKey(loot.RaidId),
        ];
    }
}