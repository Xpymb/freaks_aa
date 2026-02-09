using Freaks.Dal.Common.Implementations;
using Freaks.Dal.Common.Interfaces;
using Freaks.Dal.Common.ValueObjects;
using Freaks.Portal.Contracts.Entities.SalarySummary;
using Freaks.Portal.Dal.Interfaces.SalarySummary;
using Freaks.Portal.Dal.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Freaks.Portal.Dal.Implementation.SalarySummary;

/// <summary>
///     Провайдер для работы с проданным лутом за зарплатный период.
/// </summary>
public class SalaryLootProvider : BaseCachedProvider<SalaryLoot, long, IPortalDbContext>, ISalaryLootProvider
{
    /// <summary>
    ///     Инициализирует новый экземпляр класса <see cref="SalaryLootProvider"/>.
    /// </summary>
    /// <param name="dbContext">Контекст базы данных портала.</param>
    /// <param name="cacheProvider">Провайдер кэша.</param>
    public SalaryLootProvider(IPortalDbContext dbContext, ICacheProvider cacheProvider)
        : base(dbContext, cacheProvider)
    {
    }

    public override async Task<SalaryLoot?> GetAsync(long key, EntityTrackingType trackingType)
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
                .Include(x => x.LootItem)
                .FirstOrDefaultAsync(x => x.Id == key);

        await SetCachedValueAsync(result, TimeSpan.FromMinutes(5));
        return result;
    }

    /// <inheritdoc />
    public async Task<IList<SalaryLoot>> GetBySalaryIdAsync(long salaryId)
    {
        var cacheKey = GetCacheSalaryKey(salaryId);
        var cachedValue = await GetCachedValueAsync<IList<SalaryLoot>>(cacheKey);
        if (cachedValue is not null)
        {
            return cachedValue;
        }

        var result = await Set
            .AsNoTracking()
            .Include(x => x.LootItem)
            .Where(x => x.SalaryId == salaryId)
            .OrderBy(x => x.LootItem!.Type)
            .ThenBy(x => x.LootItem!.SynthesisExp)
            .ToListAsync();

        await SetCachedValueAsync(cacheKey, result, TimeSpan.FromMinutes(5));
        return result;
    }

    /// <inheritdoc />
    public async Task DeleteBySalaryIdAsync(long salaryId)
    {
        await RemoveCacheAsync(salaryId);
        await Set
            .Where(l => l.SalaryId == salaryId)
            .ExecuteDeleteAsync();
    }

    /// <inheritdoc />
    protected override string GetCacheKey(long key)
    {
        return $"{nameof(SalaryLoot)}:{key}";
    }

    /// <summary>
    ///     Генерирует ключ кэша для списка лута зарплатного периода.
    /// </summary>
    /// <param name="salaryId">Идентификатор зарплатного периода.</param>
    /// <returns>Строковой ключ кэша.</returns>
    private static string GetCacheSalaryKey(long salaryId)
    {
        return $"{nameof(SalaryLoot)}:list:salary:{salaryId}";
    }

    /// <inheritdoc />
    protected override List<string> GetAllCacheKeys(SalaryLoot entity)
    {
        return
        [
            GetCacheKey(entity.Id),
            GetCacheSalaryKey(entity.SalaryId),
        ];
    }

    /// <inheritdoc />
    protected override List<string> GetAllCachePrefixes(SalaryLoot entity)
    {
        return [];
    }
}