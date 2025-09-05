using Freaks.Portal.SharedContracts.Dto.RaidSummary;
using Freaks.Portal.SharedContracts.Requests.RaidSummary.Raid;
using Freaks.SharedContracts.Common;

namespace Freaks.Portal.Bll.Interfaces.RaidSummary;

/// <summary>
///     Сервис для управления рейдами и выполнения CRUD-операций.
///     Отвечает за бизнес-логику, связанную с созданием, редактированием, удалением и получением рейдов.
/// </summary>
public interface IRaidService
{
    /// <summary>
    ///     Получает подробную информацию о рейде по его идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор рейда.</param>
    /// <returns>Детальная информация о рейде.</returns>
    Task<RaidDto> GetAsync(long id);

    /// <summary>
    ///     Получает список рейдов с фильтрацией, сортировкой и пагинацией.
    /// </summary>
    /// <param name="request">Параметры запроса для фильтрации и сортировки рейдов.</param>
    /// <returns>Постраничный список кратких описаний рейдов.</returns>
    Task<PaginatedList<RaidShortDto>> GetListAsync(GetRaidListRequest request);

    /// <summary>
    ///     Создаёт новый рейд.
    /// </summary>
    /// <param name="request">Данные для создания рейда.</param>
    /// <returns>Созданный рейд с полной информацией.</returns>
    Task<RaidDto> CreateAsync(CreateRaidRequest request);

    /// <summary>
    ///     Обновляет существующий рейд.
    /// </summary>
    /// <param name="id">Идентификатор рейда</param>
    /// <param name="request">Данные для обновления рейда.</param>
    /// <returns>Обновлённый рейд с полной информацией.</returns>
    Task<RaidDto> UpdateAsync(long id, UpdateRaidRequest request);
    
    /// <summary>
    ///     Обновляет существующий рейд.
    /// </summary>
    /// <param name="id">Идентификатор рейда</param>
    /// <returns>Обновлённый рейд с полной информацией.</returns>
    Task<RaidDto> FinishAsync(long id);

    /// <summary>
    ///     Удаляет рейд по его идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор рейда для удаления.</param>
    Task DeleteAsync(long id);
}