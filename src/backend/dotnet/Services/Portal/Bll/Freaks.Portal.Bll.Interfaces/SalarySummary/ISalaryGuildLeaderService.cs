using Freaks.Portal.SharedContracts.Dto.SalarySummary;
using Freaks.Portal.SharedContracts.Requests.SalarySummary.SalaryGuildLeader;

namespace Freaks.Portal.Bll.Interfaces.SalarySummary;

/// <summary>
///     Сервис для управления долями руководства гильдии в зарплатных периодах.
///     Предоставляет методы для получения, добавления, обновления и удаления долей, связанных с конкретным зарплатным периодом.
/// </summary>
public interface ISalaryGuildLeaderService
{
    /// <summary>
    ///     Возвращает список долей руководства гильдии за указанный зарплатный период.
    /// </summary>
    /// <param name="salaryId">Идентификатор зарплатного периода.</param>
    /// <returns>Список долей руководства гильдии в виде DTO.</returns>
    Task<IList<SalaryGuildLeaderDto>> GetListAsync(long salaryId);

    /// <summary>
    ///     Добавляет новую долю руководства гильдии в зарплатный период.
    /// </summary>
    /// <param name="salaryId">Идентификатор зарплатного периода.</param>
    /// <param name="request">Запрос с информацией о доле руководства.</param>
    /// <returns>Добавленная доля руководства гильдии в виде DTO.</returns>
    Task<SalaryGuildLeaderDto> CreateAsync(long salaryId, CreateSalaryGuildLeaderRequest request);

    /// <summary>
    ///     Полностью заменяет список долей руководства гильдии в зарплатном периоде.
    ///     Все существующие записи удаляются и создаются заново на основе переданного запроса.
    /// </summary>
    /// <param name="salaryId">Идентификатор зарплатного периода.</param>
    /// <param name="request">Запрос с новым списком долей руководства гильдии.</param>
    /// <returns>Актуальный список долей руководства гильдии в виде DTO.</returns>
    Task<IList<SalaryGuildLeaderDto>> SetAsync(long salaryId, SetSalaryGuildLeaderRequest request);

    /// <summary>
    ///     Обновляет информацию о доле руководства гильдии в зарплатном периоде.
    ///     Amount пересчитывается по формуле: Quantity * PricePerLoot.
    /// </summary>
    /// <param name="salaryId">Идентификатор зарплатного периода.</param>
    /// <param name="salaryLootId">Идентификатор записи доли руководства гильдии.</param>
    /// <param name="request">Запрос с новой информацией о доле руководства.</param>
    /// <returns>Обновлённая доля руководства гильдии в виде DTO.</returns>
    Task<SalaryGuildLeaderDto> UpdateAsync(long salaryId, long salaryLootId, UpdateSalaryGuildLeaderRequest request);

    /// <summary>
    ///     Удаляет запись о доле руководства гильдии из зарплатного периода.
    /// </summary>
    /// <param name="salaryId">Идентификатор зарплатного периода.</param>
    /// <param name="salaryLootId">Идентификатор записи доли руководства гильдии для удаления.</param>
    Task DeleteAsync(long salaryId, long salaryLootId);
}
