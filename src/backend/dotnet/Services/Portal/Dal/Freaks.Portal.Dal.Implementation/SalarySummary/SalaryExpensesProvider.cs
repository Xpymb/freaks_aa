using Freaks.Dal.Common.Implementations;
using Freaks.Dal.Common.Interfaces;
using Freaks.Portal.Contracts.Entities.SalarySummary;
using Freaks.Portal.Dal.Interfaces.SalarySummary;
using Freaks.Portal.Dal.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Freaks.Portal.Dal.Implementation.SalarySummary;

/// <summary>
///     Провайдер для работы с расходами и отчислениями зарплатного периода.
/// </summary>
public class SalaryExpensesProvider : BaseCachedCompositeProvider<SalaryExpenses, SalaryExpensesKey, IPortalDbContext>, ISalaryExpensesProvider
{
    /// <summary>
    ///     Инициализирует новый экземпляр класса <see cref="SalaryExpensesProvider"/>.
    /// </summary>
    /// <param name="dbContext">Контекст базы данных портала.</param>
    /// <param name="cacheProvider">Провайдер кэша.</param>
    public SalaryExpensesProvider(IPortalDbContext dbContext, ICacheProvider cacheProvider)
        : base(dbContext, cacheProvider)
    {
    }

    /// <inheritdoc />
    public async Task<IList<SalaryExpenses>> GetBySalaryIdAsync(long salaryId)
    {
        var cacheKey = GetCacheSalaryKey(salaryId);
        var cachedValue = await GetCachedValueAsync<IList<SalaryExpenses>>(cacheKey);
        if (cachedValue is not null)
        {
            return cachedValue;
        }

        var result = await Set
            .AsNoTracking()
            .Where(x => x.SalaryId == salaryId)
            .ToListAsync();

        await SetCachedValueAsync(cacheKey, result, TimeSpan.FromMinutes(5));
        return result;
    }

    /// <inheritdoc />
    protected override IQueryable<SalaryExpenses> FilterByKey(SalaryExpensesKey key, IQueryable<SalaryExpenses> queryable)
    {
        return queryable.Where(se => se.SalaryId == key.SalaryId && se.ExpensesType == key.ExpensesType);
    }

    /// <inheritdoc />
    protected override string GetCacheKey(SalaryExpensesKey key)
    {
        return $"{nameof(SalaryExpenses)}:salary:{key.SalaryId}:type:{key.ExpensesType}";
    }

    /// <summary>
    ///     Генерирует ключ кэша для списка расходов зарплатного периода.
    /// </summary>
    /// <param name="salaryId">Идентификатор зарплатного периода.</param>
    /// <returns>Строковой ключ кэша.</returns>
    private static string GetCacheSalaryKey(long salaryId)
    {
        return $"{nameof(SalaryExpenses)}:list:salary:{salaryId}";
    }

    /// <inheritdoc />
    protected override List<string> GetAllCacheKeys(SalaryExpenses entity)
    {
        return
        [
            GetCacheKey(entity.GetCompositeKey()),
            GetCacheSalaryKey(entity.SalaryId),
        ];
    }

    /// <inheritdoc />
    protected override List<string> GetAllCachePrefixes(SalaryExpenses entity)
    {
        return [];
    }
}