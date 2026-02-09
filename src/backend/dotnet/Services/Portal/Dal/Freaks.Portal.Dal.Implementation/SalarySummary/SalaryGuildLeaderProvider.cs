using Freaks.Dal.Common.Implementations;
using Freaks.Dal.Common.Interfaces;
using Freaks.Dal.Common.ValueObjects;
using Freaks.Portal.Contracts.Entities.SalarySummary;
using Freaks.Portal.Dal.Interfaces.SalarySummary;
using Freaks.Portal.Dal.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Freaks.Portal.Dal.Implementation.SalarySummary;

/// <summary>
///     Провайдер для работы с долями руководства гильдии в зарплате.
/// </summary>
public class SalaryGuildLeaderProvider : BaseCachedProvider<SalaryGuildLeader, long, IPortalDbContext>, ISalaryGuildLeaderProvider
{
    /// <summary>
    ///     Инициализирует новый экземпляр класса <see cref="SalaryGuildLeaderProvider"/>.
    /// </summary>
    /// <param name="dbContext">Контекст базы данных портала.</param>
    /// <param name="cacheProvider">Провайдер кэша.</param>
    public SalaryGuildLeaderProvider(IPortalDbContext dbContext, ICacheProvider cacheProvider) : base(dbContext, cacheProvider)
    {
    }

    public override async Task<SalaryGuildLeader?> GetAsync(long key, EntityTrackingType trackingType)
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

        var query = Set.AsQueryable();

        if (trackingType is EntityTrackingType.NoTracking)
        {
            query = query.AsNoTracking();
        }

        var result =
            await query
                .Include(x => x.SalaryLoot)
                .ThenInclude(x => x!.LootItem)
                .FirstOrDefaultAsync(x => x.Id == key);

        await SetCachedValueAsync(result, TimeSpan.FromMinutes(5));
        return result;
    }

    /// <inheritdoc />
    public async Task<IList<SalaryGuildLeader>> GetBySalaryIdAsync(long salaryId)
    {
        var cacheKey = GetCacheKey(salaryId);
        var cachedValue = await GetCachedValueAsync<IList<SalaryGuildLeader>>(cacheKey);
        if (cachedValue is not null)
        {
            return cachedValue;
        }

        var result = await Set
            .AsNoTracking()
            .Include(x => x.SalaryLoot)
            .ThenInclude(x => x!.LootItem)
            .Where(x => x.SalaryId == salaryId)
            .OrderBy(x => x.SalaryLoot!.LootItem!.Type)
            .ToListAsync();

        await SetCachedValueAsync(cacheKey, result, TimeSpan.FromMinutes(5));
        return result;
    }

    /// <inheritdoc />
    protected override string GetCacheKey(long key)
    {
        return $"{nameof(SalaryGuildLeader)}:{key}";
    }

    /// <summary>
    ///     Генерирует ключ кэша для списка долей руководства зарплатного периода.
    /// </summary>
    /// <param name="salaryId">Идентификатор зарплатного периода.</param>
    /// <returns>Строковой ключ кэша.</returns>
    private static string GetCacheSalaryKey(long salaryId)
    {
        return $"{nameof(SalaryGuildLeader)}:list:salary:{salaryId}";
    }

    /// <inheritdoc />
    protected override List<string> GetAllCacheKeys(SalaryGuildLeader entity)
    {
        return
        [
            GetCacheKey(entity.Id),
            GetCacheSalaryKey(entity.SalaryId),
        ];
    }

    /// <inheritdoc />
    protected override List<string> GetAllCachePrefixes(SalaryGuildLeader entity)
    {
        return [];
    }
}