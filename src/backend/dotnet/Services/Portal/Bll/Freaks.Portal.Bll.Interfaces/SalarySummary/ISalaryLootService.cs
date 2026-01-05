using Freaks.Portal.SharedContracts.Dto.SalarySummary;
using Freaks.Portal.SharedContracts.Requests.SalarySummary.SalaryLoot;

namespace Freaks.Portal.Bll.Interfaces.SalarySummary;

/// <summary>
///     Сервис для управления проданным лутом за зарплатные периоды.
///     Предоставляет методы для получения, добавления, обновления и удаления лута, связанного с конкретным зарплатным периодом.
/// </summary>
public interface ISalaryLootService
{
    /// <summary>
    ///     Возвращает список проданного лута за указанный зарплатный период.
    /// </summary>
    /// <param name="salaryId">Идентификатор зарплатного периода.</param>
    /// <returns>Список проданного лута в виде DTO.</returns>
    Task<IList<SalaryLootDto>> GetListAsync(long salaryId);

    /// <summary>
    ///     Добавляет новый проданный лут в зарплатный период.
    /// </summary>
    /// <param name="salaryId">Идентификатор зарплатного периода.</param>
    /// <param name="request">Запрос с информацией о луте и количестве.</param>
    /// <returns>Добавленный проданный лут в виде DTO.</returns>
    Task<SalaryLootDto> CreateAsync(long salaryId, CreateSalaryLootRequest request);

    /// <summary>
    ///     Обновляет информацию о проданном луте в зарплатном периоде.
    ///     Поддерживает обратный пересчёт: если изменяется DiscountPercent, пересчитывается Amount и наоборот.
    /// </summary>
    /// <param name="salaryId">Идентификатор зарплатного периода.</param>
    /// <param name="lootId">Идентификатор записи проданного лута.</param>
    /// <param name="request">Запрос с новой информацией о луте.</param>
    /// <returns>Обновлённый проданный лут в виде DTO.</returns>
    Task<SalaryLootDto> UpdateAsync(long salaryId, long lootId, UpdateSalaryLootRequest request);

    /// <summary>
    ///     Удаляет запись о проданном луте из зарплатного периода.
    /// </summary>
    /// <param name="salaryId">Идентификатор зарплатного периода.</param>
    /// <param name="lootId">Идентификатор записи проданного лута для удаления.</param>
    Task DeleteAsync(long salaryId, long lootId);
}
