using Freaks.Dal.Common.Implementations;
using Freaks.Dal.Common.Interfaces;
using Freaks.Dal.Common.ValueObjects;
using Freaks.Portal.Contracts.Entities.SalarySummary;
using Freaks.Portal.Dal.Interfaces.SalarySummary;
using Freaks.Portal.Dal.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Freaks.Portal.Dal.Implementation.SalarySummary;

/// <summary>
///     Провайдер для работы с участниками зарплатного периода.
/// </summary>
public class SalaryMemberProvider : BaseCachedCompositeProvider<SalaryMember, SalaryMemberKey, IPortalDbContext>, ISalaryMemberProvider
{
    /// <summary>
    ///     Инициализирует новый экземпляр класса <see cref="SalaryMemberProvider"/>.
    /// </summary>
    /// <param name="dbContext">Контекст базы данных портала.</param>
    /// <param name="cacheProvider">Провайдер кэша.</param>
    public SalaryMemberProvider(IPortalDbContext dbContext, ICacheProvider cacheProvider)
        : base(dbContext, cacheProvider)
    {
    }

    public override async Task<SalaryMember?> GetAsync(SalaryMemberKey key, EntityTrackingType trackingType)
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
                .Include(x => x.User)
                .FirstOrDefaultAsync();

        await SetCachedValueAsync(result, TimeSpan.FromMinutes(5));
        return result;
    }

    /// <inheritdoc />
    public async Task<IList<SalaryMember>> GetBySalaryIdAsync(long salaryId)
    {
        var cacheKey = GetCacheSalaryKey(salaryId);
        var cachedValue = await GetCachedValueAsync<IList<SalaryMember>>(cacheKey);
        if (cachedValue is not null)
        {
            return cachedValue;
        }

        var result = await Set
            .AsNoTracking()
            .Include(sm => sm.User)
            .Where(x => x.SalaryId == salaryId)
            .OrderByDescending(x => x.ActivityGold)
            .ToListAsync();

        await SetCachedValueAsync(cacheKey, result, TimeSpan.FromMinutes(5));
        return result;
    }

    /// <inheritdoc />
    protected override IQueryable<SalaryMember> FilterByKey(SalaryMemberKey key, IQueryable<SalaryMember> queryable)
    {
        return queryable.Where(sm => sm.SalaryId == key.SalaryId && sm.UserId == key.UserId);
    }

    /// <inheritdoc />
    protected override string GetCacheKey(SalaryMemberKey key)
    {
        return $"{nameof(SalaryMember)}:salary:{key.SalaryId}:user:{key.UserId}";
    }

    /// <summary>
    ///     Генерирует ключ кэша для списка участников зарплатного периода.
    /// </summary>
    /// <param name="salaryId">Идентификатор зарплатного периода.</param>
    /// <returns>Строковой ключ кэша.</returns>
    private static string GetCacheSalaryKey(long salaryId)
    {
        return $"{nameof(SalaryMember)}:list:salary:{salaryId}";
    }

    /// <inheritdoc />
    protected override List<string> GetAllCacheKeys(SalaryMember entity)
    {
        return
        [
            GetCacheKey(entity.GetCompositeKey()),
            GetCacheSalaryKey(entity.SalaryId),
        ];
    }

    /// <inheritdoc />
    protected override List<string> GetAllCachePrefixes(SalaryMember entity)
    {
        return [];
    }
}