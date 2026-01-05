using Freaks.Dal.Common.Interfaces;
using Freaks.Portal.Contracts.Entities.SalarySummary;
using Freaks.Portal.Dal.Persistence;
using Freaks.Portal.SharedContracts.Requests.SalarySummary.Salary;
using Freaks.SharedContracts.Common;

namespace Freaks.Portal.Dal.Interfaces.SalarySummary;

/// <summary>
///     Провайдер для работы с зарплатными периодами.
/// </summary>
public interface ISalaryProvider : IBaseProvider<Salary, long, IPortalDbContext>
{
    /// <summary>
    ///     Возвращает список зарплатных периодов с постраничной выборкой на основе переданных фильтров и параметров сортировки.
    /// </summary>
    /// <param name="request">Параметры фильтрации, сортировки и пагинации.</param>
    /// <returns>Постраничный список зарплатных периодов, соответствующих условиям запроса.</returns>
    Task<PaginatedList<Salary>> GetPaginatedListAsync(GetSalaryListRequest request);
}