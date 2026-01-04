using Freaks.Dal.Common.Implementations;
using Freaks.Dal.Common.Interfaces;
using Freaks.Portal.Contracts.Entities.SalarySummary;
using Freaks.Portal.Dal.Interfaces.SalarySummary;
using Freaks.Portal.Dal.Persistence;

namespace Freaks.Portal.Dal.Implementation.SalarySummary;

/// <summary>
///     Провайдер для работы с параметрами зарплатного периода.
/// </summary>
public class SalaryParametersProvider : BaseCachedProvider<SalaryParameters, long, IPortalDbContext>, ISalaryParametersProvider
{
    /// <summary>
    ///     Инициализирует новый экземпляр класса <see cref="SalaryParametersProvider"/>.
    /// </summary>
    /// <param name="dbContext">Контекст базы данных портала.</param>
    /// <param name="cacheProvider">Провайдер кэша.</param>
    public SalaryParametersProvider(IPortalDbContext dbContext, ICacheProvider cacheProvider)
        : base(dbContext, cacheProvider)
    {
    }

    /// <inheritdoc />
    protected override string GetCacheKey(long key)
    {
        return $"{nameof(SalaryParameters)}:{key}";
    }

    /// <inheritdoc />
    protected override List<string> GetAllCacheKeys(SalaryParameters entity)
    {
        return
        [
            GetCacheKey(entity.Id),
        ];
    }

    /// <inheritdoc />
    protected override List<string> GetAllCachePrefixes(SalaryParameters entity)
    {
        return [];
    }
}