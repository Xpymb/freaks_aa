using System.Text.Json;
using Freaks.Dal.Common.Extensions;
using Freaks.Dal.Common.Implementations;
using Freaks.Dal.Common.Interfaces;
using Freaks.Portal.Contracts.Entities.SalarySummary;
using Freaks.Portal.Dal.Interfaces.SalarySummary;
using Freaks.Portal.Dal.Persistence;
using Freaks.Portal.SharedContracts.Requests.SalarySummary.Salary;
using Freaks.SharedContracts.Common;
using Microsoft.EntityFrameworkCore;

namespace Freaks.Portal.Dal.Implementation.SalarySummary;

/// <summary>
///     Провайдер для работы с зарплатными периодами.
/// </summary>
public class SalaryProvider : BaseCachedProvider<Salary, long, IPortalDbContext>, ISalaryProvider
{
    /// <summary>
    ///     Инициализирует новый экземпляр класса <see cref="SalaryProvider"/>.
    /// </summary>
    /// <param name="dbContext">Контекст базы данных портала.</param>
    /// <param name="cacheProvider">Провайдер кэша.</param>
    public SalaryProvider(IPortalDbContext dbContext, ICacheProvider cacheProvider)
        : base(dbContext, cacheProvider)
    {
    }

    /// <inheritdoc />
    public async Task<PaginatedList<Salary>> GetPaginatedListAsync(GetSalaryListRequest request)
    {
        var cacheKey = GetParameterizedCacheKey(request);
        var cachedValue = await GetCachedValueAsync<PaginatedList<Salary>>(cacheKey);
        if (cachedValue is not null)
        {
            return cachedValue;
        }

        var query = Set.AsNoTracking();

        if (request.From is not null)
        {
            query = query.Where(s => s.StartDt >= request.From);
        }

        if (request.To is not null)
        {
            query = query.Where(s => s.EndDt <= request.To);
        }

        query = request.SortBy switch
        {
            SalaryListSortByType.Id => query.OrderBy(s => s.Id, request.SortMode),
            SalaryListSortByType.Name => query.OrderBy(s => s.Name, request.SortMode),
            SalaryListSortByType.FillStatus => query.OrderBy(s => s.FillStatus, request.SortMode),
            SalaryListSortByType.RegistrationStatus => query.OrderBy(s => s.RegistrationStatus, request.SortMode),
            _ => throw new ArgumentOutOfRangeException(),
        };

        var resultCount = await query.CountAsync();
        var result = await query
            .UseTakeSkip(request.Take, request.Skip)
            .ToListAsync();

        var paginatedResult = new PaginatedList<Salary>(result, request.Take, request.Skip, resultCount);

        await SetCachedValueAsync(cacheKey, paginatedResult, TimeSpan.FromMinutes(5));
        return paginatedResult;
    }

    /// <inheritdoc />
    protected override string GetCacheKey(long key)
    {
        return $"{nameof(Salary)}:{key}";
    }

    /// <inheritdoc />
    protected override List<string> GetAllCacheKeys(Salary entity)
    {
        return
        [
            GetCacheKey(entity.Id),
        ];
    }

    /// <inheritdoc />
    protected override List<string> GetAllCachePrefixes(Salary entity)
    {
        return
        [
            GetDefaultCachePrefix(),
        ];
    }

    /// <summary>
    ///     Возвращает стандартный префикс кэша для запросов списка зарплатных периодов.
    /// </summary>
    private static string GetDefaultCachePrefix()
    {
        return $"{nameof(Salary)}:list";
    }

    /// <summary>
    ///     Генерирует параметризированный ключ кэша на основе запроса <see cref="GetSalaryListRequest"/>.
    /// </summary>
    /// <param name="parameters">Параметры запроса списка зарплатных периодов.</param>
    /// <returns>Строковой ключ кэша.</returns>
    private static string GetParameterizedCacheKey(GetSalaryListRequest parameters)
    {
        var parametersJson = JsonSerializer.Serialize(parameters);
        return $"{GetDefaultCachePrefix()}:{parametersJson}";
    }
}