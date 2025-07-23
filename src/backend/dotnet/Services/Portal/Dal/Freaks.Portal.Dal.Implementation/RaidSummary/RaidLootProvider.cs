using Freaks.Dal.Common.Interfaces;
using Freaks.Dal.Common.ValueObjects;
using Freaks.Portal.Contracts.Entities.RaidSummary;
using Freaks.Portal.Dal.Interfaces.RaidSummary;
using Freaks.Portal.Dal.Persistence;
using Freaks.WebApi.Common.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Freaks.Portal.Dal.Implementation.RaidSummary;

/// <summary>
/// Провайдер доступа к данным о луте, полученном в рейдах.
/// Обеспечивает взаимодействие с базой данных и кэшированием для операций получения, создания, обновления и удаления лута.
/// </summary>
public class RaidLootProvider : IRaidLootProvider
{
    private readonly ICacheProvider _cacheProvider;
    private readonly IPortalDbContext _dbContext;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="RaidLootProvider"/>.
    /// </summary>
    /// <param name="cacheProvider">Провайдер кэширования.</param>
    /// <param name="portalDbContext">Контекст базы данных портала.</param>
    /// <exception cref="ArgumentNullException">Выбрасывается, если один из аргументов равен null.</exception>
    public RaidLootProvider(
        ICacheProvider cacheProvider,
        IPortalDbContext portalDbContext)
    {
        _cacheProvider = cacheProvider ?? throw new ArgumentNullException(nameof(cacheProvider));
        _dbContext = portalDbContext ?? throw new ArgumentNullException(nameof(portalDbContext));
    }

    /// <inheritdoc />
    public async Task<RaidLoot?> GetAsync(int raidId, int lootId, EntityTrackingType trackingType)
    {
        var query = _dbContext.RaidLoots.Where(l => (l.RaidId == raidId) && (l.LootId == lootId));

        if (trackingType is EntityTrackingType.NoTracking)
        {
            query = query.AsNoTracking();
        }

        return await query.FirstOrDefaultAsync();
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
    public async Task<RaidLoot> UpdateAsync(RaidLoot loot)
    {
        var originalEntity = await GetAsync(loot.RaidId, loot.LootId, EntityTrackingType.Tracking);
        if (originalEntity is null)
        {
            throw new EntityNotFoundException();
        }

        await RemoveCacheAsync(loot);

        var entry = _dbContext.RaidLoots.Entry(originalEntity);
        entry.CurrentValues.SetValues(loot);
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

    /// <summary>
    /// Удаляет все кэш-записи, связанные с указанным лутом.
    /// </summary>
    /// <param name="loot">Объект лута, для которого нужно очистить кэш.</param>
    private async Task RemoveCacheAsync(RaidLoot loot)
    {
        var cacheKeys = GetAllCacheKeys(loot);
        await _cacheProvider.RemoveAsync(cacheKeys);
    }

    /// <summary>
    /// Возвращает ключ кэша для конкретной записи лута.
    /// </summary>
    /// <param name="raidId">Идентификатор рейда.</param>
    /// <param name="lootId">Идентификатор предмета лута.</param>
    /// <returns>Ключ кэша.</returns>
    private static string GetCacheKey(int raidId, int lootId)
    {
        return $"{nameof(RaidLoot)}:raid:{raidId}:loot:{lootId}";
    }
    
    /// <summary>
    /// Возвращает ключ кэша для списка лута, связанного с рейдом.
    /// </summary>
    /// <param name="raidId">Идентификатор рейда.</param>
    /// <returns>Ключ кэша.</returns>
    private static string GetCacheRaidKey(int raidId)
    {
        return $"{nameof(RaidLoot)}:list:raid:{raidId}";
    }

    /// <summary>
    /// Возвращает список всех ключей кэша, связанных с объектом лута.
    /// </summary>
    /// <param name="loot">Объект лута.</param>
    /// <returns>Список ключей кэша.</returns>
    private static List<string> GetAllCacheKeys(RaidLoot loot)
    {
        return
        [
            GetCacheKey(loot.RaidId, loot.LootId),
            GetCacheRaidKey(loot.RaidId),
        ];
    }
}