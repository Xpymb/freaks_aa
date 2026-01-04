using Freaks.Dal.Common.Interfaces;
using Freaks.Portal.Contracts.Entities.SalarySummary;
using Freaks.Portal.Dal.Persistence;

namespace Freaks.Portal.Dal.Interfaces.SalarySummary;

/// <summary>
///     Провайдер для работы с участниками зарплатного периода.
/// </summary>
public interface ISalaryMemberProvider : IBaseCompositeProvider<SalaryMember, SalaryMemberKey, IPortalDbContext>
{
    /// <summary>
    ///     Возвращает список участников по идентификатору зарплатного периода.
    /// </summary>
    /// <param name="salaryId">Идентификатор зарплатного периода.</param>
    /// <returns>Список участников зарплатного периода.</returns>
    Task<IList<SalaryMember>> GetBySalaryIdAsync(long salaryId);
}