using Freaks.Dal.Common.Interfaces;
using Freaks.Portal.Contracts.Entities.SalarySummary;
using Freaks.Portal.Dal.Persistence;

namespace Freaks.Portal.Dal.Interfaces.SalarySummary;

/// <summary>
///     Провайдер для работы с расходами и отчислениями зарплатного периода.
/// </summary>
public interface ISalaryExpensesProvider : IBaseProvider<SalaryExpenses, long, IPortalDbContext>
{
    /// <summary>
    ///     Возвращает список расходов и отчислений по идентификатору зарплатного периода.
    /// </summary>
    /// <param name="salaryId">Идентификатор зарплатного периода.</param>
    /// <returns>Список расходов и отчислений зарплатного периода.</returns>
    Task<IList<SalaryExpenses>> GetBySalaryIdAsync(long salaryId);
}