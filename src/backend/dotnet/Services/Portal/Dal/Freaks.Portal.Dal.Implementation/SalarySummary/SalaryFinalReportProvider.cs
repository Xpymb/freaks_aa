using Freaks.Dal.Common.Implementations;
using Freaks.Dal.Common.Interfaces;
using Freaks.Portal.Contracts.Entities.SalarySummary;
using Freaks.Portal.Dal.Interfaces.SalarySummary;
using Freaks.Portal.Dal.Persistence;

namespace Freaks.Portal.Dal.Implementation.SalarySummary;

/// <summary>
///     Провайдер для работы с итоговыми отчётами зарплатных периодов.
///     Использует кэширование для ускорения доступа к данным.
/// </summary>
public class SalaryFinalReportProvider : BaseCachedProvider<SalaryFinalReport, long, IPortalDbContext>, ISalaryFinalReportProvider
{
    /// <summary>
    ///     Инициализирует новый экземпляр провайдера <see cref="SalaryFinalReportProvider" />.
    /// </summary>
    /// <param name="dbContext">Контекст базы данных портала.</param>
    /// <param name="cacheProvider">Провайдер кэширования.</param>
    public SalaryFinalReportProvider(IPortalDbContext dbContext, ICacheProvider cacheProvider) : base(dbContext, cacheProvider)
    {
    }

    /// <inheritdoc />
    protected override string GetCacheKey(long key)
    {
        return $"{nameof(SalaryFinalReport)}:{key}";
    }

    /// <inheritdoc />
    protected override List<string> GetAllCacheKeys(SalaryFinalReport entity)
    {
        return
        [
            GetCacheKey(entity.Id)
        ];
    }

    /// <inheritdoc />
    protected override List<string> GetAllCachePrefixes(SalaryFinalReport entity)
    {
        return [];
    }
}