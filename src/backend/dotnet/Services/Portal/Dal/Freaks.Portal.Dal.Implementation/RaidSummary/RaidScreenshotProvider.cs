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
public class RaidScreenshotProvider : IRaidScreenshotProvider
{
    private readonly ICacheProvider _cacheProvider;
    private readonly IPortalDbContext _dbContext;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="RaidScreenshotProvider"/>.
    /// </summary>
    /// <param name="cacheProvider">Провайдер кэша.</param>
    /// <param name="portalDbContext">Контекст базы данных портала.</param>
    public RaidScreenshotProvider(
        ICacheProvider cacheProvider, 
        IPortalDbContext portalDbContext)
    {
        _cacheProvider = cacheProvider ?? throw new ArgumentNullException(nameof(cacheProvider));
        _dbContext = portalDbContext ?? throw new ArgumentNullException(nameof(portalDbContext));
    }

    /// <inheritdoc />
    public async Task<RaidScreenshot?> GetAsync(int raidId, string screenshotUrl)
    {
        return await _dbContext.RaidScreenshots
                               .AsNoTracking()
                               .FirstOrDefaultAsync(s => (s.RaidId == raidId) && (s.ScreenshotUrl == screenshotUrl));
    }

    /// <inheritdoc />
    public async Task<IList<RaidScreenshot>> GetByRaidIdAsync(int raidId)
    {
        var cacheKey = GetCacheRaidKey(raidId);
        var cachedValue = await _cacheProvider.GetAsync<IList<RaidScreenshot>>(cacheKey);
        if (cachedValue is not null)
        {
            return cachedValue;
        }

        var result = await _dbContext.RaidScreenshots
                                     .AsNoTracking()
                                     .Where(r => r.RaidId == raidId)
                                     .ToListAsync();

        await _cacheProvider.SetAsync(cacheKey, result, TimeSpan.FromMinutes(5));
        return result;
    }

    /// <inheritdoc />
    public async Task<IList<RaidScreenshot>> SetAsync(IList<RaidScreenshot> screenshots)
    {
        await RemoveCacheAsync(screenshots);

        await _dbContext.RaidScreenshots.BulkInsertAsync(screenshots);

        return screenshots;
    }

    /// <inheritdoc />
    public async Task DeleteAsync(RaidScreenshot screenshot)
    {
        await RemoveCacheAsync(screenshot);

        await _dbContext.RaidScreenshots
                        .Where(s => s.RaidId == screenshot.RaidId && s.ScreenshotUrl == screenshot.ScreenshotUrl)
                        .ExecuteDeleteAsync();
    }

    /// <summary>
    /// Удаляет кэш по одному скриншоту.
    /// </summary>
    /// <param name="screenshot">Скриншот, по которому требуется сбросить кэш.</param>
    private async Task RemoveCacheAsync(RaidScreenshot screenshot)
    {
        var cacheKeys = GetAllCacheKeys(screenshot);
        await _cacheProvider.RemoveAsync(cacheKeys);
    }

    /// <summary>
    /// Удаляет кэш по списку скриншотов.
    /// </summary>
    /// <param name="screenshots">Список скриншотов для очистки кэша.</param>
    private async Task RemoveCacheAsync(IList<RaidScreenshot> screenshots)
    {
        foreach (var screenshot in screenshots)
        {
            var cacheKeys = GetAllCacheKeys(screenshot);
            await _cacheProvider.RemoveAsync(cacheKeys);
        }
    }
    
    /// <summary>
    /// Формирует ключ кэша для скриншотов, привязанных к рейду.
    /// </summary>
    /// <param name="raidId">Идентификатор рейда.</param>
    /// <returns>Строковой ключ кэша.</returns>
    private static string GetCacheRaidKey(int raidId)
    {
        return $"{nameof(RaidScreenshot)}:list:raid:{raidId}";
    }

    /// <summary>
    /// Возвращает список всех ключей кэша, связанных с конкретным скриншотом.
    /// </summary>
    /// <param name="screenshot">Скриншот рейда.</param>
    /// <returns>Список строковых ключей.</returns>
    private static List<string> GetAllCacheKeys(RaidScreenshot screenshot)
    {
        return
        [
            GetCacheRaidKey(screenshot.RaidId),
        ];
    }
}