using Freaks.Portal.SharedContracts.Dto.SalarySummary;
using Freaks.Portal.SharedContracts.Requests.SalarySummary.SalaryExpenses;
using Freaks.Portal.SharedContracts.ValueObjects.SalarySummary;

namespace Freaks.Portal.Bll.Interfaces.SalarySummary;

/// <summary>
///     Сервис для управления расходами гильдии в зарплатных периодах.
///     Предоставляет методы для получения, добавления, обновления и удаления расходов, связанных с конкретным зарплатным периодом.
/// </summary>
public interface ISalaryExpensesService
{
    /// <summary>
    ///     Возвращает список расходов гильдии за указанный зарплатный период.
    /// </summary>
    /// <param name="salaryId">Идентификатор зарплатного периода.</param>
    /// <returns>Список расходов гильдии в виде DTO.</returns>
    Task<IList<SalaryExpensesDto>> GetListAsync(long salaryId);

    /// <summary>
    ///     Добавляет новый расход гильдии в зарплатный период.
    /// </summary>
    /// <param name="salaryId">Идентификатор зарплатного периода.</param>
    /// <param name="request">Запрос с информацией о расходе.</param>
    /// <returns>Добавленный расход гильдии в виде DTO.</returns>
    Task<SalaryExpensesDto> CreateAsync(long salaryId, CreateSalaryExpensesRequest request);

    /// <summary>
    ///     Обновляет информацию о расходе гильдии в зарплатном периоде.
    /// </summary>
    /// <param name="salaryId">Идентификатор зарплатного периода.</param>
    /// <param name="expensesType">Тип расхода (часть составного ключа).</param>
    /// <param name="request">Запрос с новой информацией о расходе.</param>
    /// <returns>Обновлённый расход гильдии в виде DTO.</returns>
    Task<SalaryExpensesDto> UpdateAsync(long salaryId, SalaryExpensesType expensesType, UpdateSalaryExpensesRequest request);

    /// <summary>
    ///     Удаляет запись о расходе гильдии из зарплатного периода.
    /// </summary>
    /// <param name="salaryId">Идентификатор зарплатного периода.</param>
    /// <param name="expensesType">Тип расхода (часть составного ключа) для удаления.</param>
    Task DeleteAsync(long salaryId, SalaryExpensesType expensesType);
}
