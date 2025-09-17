using Freaks.Dal.Common.Implementations;
using Freaks.Dal.Common.Interfaces;
using Freaks.Portal.Contracts.Entities.RaidSummary;
using Freaks.Portal.Dal.Interfaces.RaidSummary;
using Freaks.Portal.Dal.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Freaks.Portal.Dal.Implementation.RaidSummary;

/// <summary>
/// Провайдер для работы со скриншотами, прикреплёнными к рейдам.
/// Обеспечивает получение, сохранение и удаление скриншотов с поддержкой кэширования.
/// </summary>
public class RaidScreenshotProvider : BaseCachedCompositeProvider<RaidScreenshot, RaidScreenshotKey, IPortalDbContext>, IRaidScreenshotProvider
{
    /// <summary>
    /// Инициализирует новый экземпляр <see cref="RaidScreenshotProvider"/>.
    /// </summary>
    /// <param name="cacheProvider">Провайдер кэша.</param>
    /// <param name="dbContext">Контекст базы данных портала.</param>
    public RaidScreenshotProvider(
        ICacheProvider cacheProvider, 
        IPortalDbContext dbContext) : base(dbContext, cacheProvider)
    {
    }
    /// <inheritdoc />
    public async Task<IList<RaidScreenshot>> GetByRaidIdAsync(long raidId)
    {
        var cacheKey = GetCacheRaidKey(raidId);
        var cachedValue = await GetCachedValueAsync<IList<RaidScreenshot>>(cacheKey);
        if (cachedValue is not null)
        {
            return cachedValue;
        }

        var result = await Set
                                     .AsNoTracking()
                                     .Where(r => r.RaidId == raidId)
                                     .ToListAsync();

        await SetCachedValueAsync(cacheKey, result, TimeSpan.FromMinutes(5));
        return result;
    }

    /// <inheritdoc />
    public async Task<int> CountByRaidAsync(long raidId)
    {
        return await Set.CountAsync(r => r.RaidId == raidId);
    }

    /// <inheritdoc />
    protected override IQueryable<RaidScreenshot> FilterByKey(RaidScreenshotKey key, IQueryable<RaidScreenshot> queryable)
    {
        return queryable.Where(s => s.RaidId == key.RaidId && s.ScreenshotUri == key.ScreenshotUrl);
    }

    /// <inheritdoc />
    protected override string GetCacheKey(RaidScreenshotKey key)
    {
        return $"{nameof(RaidScreenshot)}:raid:{key.RaidId}:screenshot:{key.ScreenshotUrl}";
    }

    /// <inheritdoc />
    protected override List<string> GetAllCacheKeys(RaidScreenshot entity)
    {
        return
        [
            GetCacheKey(entity.GetCompositeKey()),
            GetCacheRaidKey(entity.RaidId),
        ];
    }

    /// <inheritdoc />
    protected override List<string> GetAllCachePrefixes(RaidScreenshot entity)
    {
        return [];
    }
    
    /// <summary>
    /// Формирует ключ кэша для скриншотов, привязанных к рейду.
    /// </summary>
    /// <param name="raidId">Идентификатор рейда.</param>
    /// <returns>Строковой ключ кэша.</returns>
    private static string GetCacheRaidKey(long raidId)
    {
        return $"{nameof(RaidScreenshot)}:list:raid:{raidId}";
    }
}