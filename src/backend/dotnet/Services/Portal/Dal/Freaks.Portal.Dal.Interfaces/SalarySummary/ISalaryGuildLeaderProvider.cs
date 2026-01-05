using Freaks.Dal.Common.Interfaces;
using Freaks.Portal.Contracts.Entities.SalarySummary;
using Freaks.Portal.Dal.Persistence;

namespace Freaks.Portal.Dal.Interfaces.SalarySummary;

/// <summary>
///     Провайдер для работы с долями руководства гильдии в зарплате.
/// </summary>
public interface ISalaryGuildLeaderProvider : IBaseProvider<SalaryGuildLeader, long, IPortalDbContext>
{
    /// <summary>
    ///     Возвращает список долей руководства гильдии по идентификатору зарплатного периода.
    /// </summary>
    /// <param name="salaryId">Идентификатор зарплатного периода.</param>
    /// <returns>Список долей руководства гильдии в зарплате.</returns>
    Task<IList<SalaryGuildLeader>> GetBySalaryIdAsync(long salaryId);
}