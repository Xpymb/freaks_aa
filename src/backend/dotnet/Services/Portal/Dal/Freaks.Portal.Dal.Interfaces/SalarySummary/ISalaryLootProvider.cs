using Freaks.Dal.Common.Interfaces;
using Freaks.Portal.Contracts.Entities.SalarySummary;
using Freaks.Portal.Dal.Persistence;

namespace Freaks.Portal.Dal.Interfaces.SalarySummary;

/// <summary>
///     Провайдер для работы с проданным лутом за зарплатный период.
/// </summary>
public interface ISalaryLootProvider : IBaseProvider<SalaryLoot, long, IPortalDbContext>
{
    /// <summary>
    ///     Возвращает список проданного лута по идентификатору зарплатного периода.
    /// </summary>
    /// <param name="salaryId">Идентификатор зарплатного периода.</param>
    /// <returns>Список проданного лута за период.</returns>
    Task<IList<SalaryLoot>> GetBySalaryIdAsync(long salaryId);

    /// <summary>
    ///     Удаляет все записи проданного лута, связанные с указанным зарплатным периодом.
    /// </summary>
    /// <param name="salaryId">Идентификатор зарплатного периода.</param>
    Task DeleteBySalaryIdAsync(long salaryId);
}