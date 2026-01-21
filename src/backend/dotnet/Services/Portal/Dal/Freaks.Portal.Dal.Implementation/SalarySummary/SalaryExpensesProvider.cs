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
public class SalaryExpensesProvider : BaseCachedProvider<SalaryExpenses, long, IPortalDbContext>, ISalaryExpensesProvider
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
            .Include(x => x.User)
            .Where(x => x.SalaryId == salaryId)
            .OrderBy(x => x.ExpensesType)
            .ToListAsync();

        await SetCachedValueAsync(cacheKey, result, TimeSpan.FromMinutes(5));
        return result;
    }

    protected override string GetCacheKey(long key)
    {
        return $"{nameof(SalaryExpenses)}:{key}";
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
            GetCacheKey(entity.Id),
            GetCacheSalaryKey(entity.SalaryId),
        ];
    }

    /// <inheritdoc />
    protected override List<string> GetAllCachePrefixes(SalaryExpenses entity)
    {
        return [];
    }
}