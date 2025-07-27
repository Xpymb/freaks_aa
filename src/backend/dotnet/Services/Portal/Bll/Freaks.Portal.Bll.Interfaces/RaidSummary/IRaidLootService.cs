using Freaks.Portal.SharedContracts.Dto.RaidSummary;
using Freaks.Portal.SharedContracts.Requests.RaidSummary.RaidLoot;

namespace Freaks.Portal.Bll.Interfaces.RaidSummary;

/// <summary>
///     Сервис для управления лутом, полученным в рейдах.
///     Предоставляет методы для получения, добавления, обновления и удаления лута, связанного с конкретным рейдом.
/// </summary>
public interface IRaidLootService
{
    /// <summary>
    ///     Возвращает список лута, полученного в указанном рейде.
    /// </summary>
    /// <param name="raidId">Идентификатор рейда.</param>
    /// <returns>Список предметов лута в виде DTO.</returns>
    Task<IList<RaidLootDto>> GetListAsync(long raidId);

    /// <summary>
    ///     Добавляет новый предмет лута в рейд.
    /// </summary>
    /// <param name="raidId">Идентификатор рейда.</param>
    /// <param name="request">Запрос с информацией о рейде, предмете и количестве.</param>
    /// <returns>Добавленный предмет лута в виде DTO.</returns>
    Task<RaidLootDto> CreateAsync(long raidId, CreateRaidLootRequest request);

    /// <summary>
    ///     Обновляет количество указанного предмета лута в рейде.
    /// </summary>
    /// <param name="raidId">Идентификатор рейда.</param>
    /// <param name="lootId">Идентификатор предмета лута.</param>
    /// <param name="request">Запрос с новой информацией о луте.</param>
    /// <returns>Обновлённый предмет лута в виде DTO.</returns>
    Task<RaidLootDto> UpdateAsync(long raidId, int lootId, UpdateRaidLootRequest request);

    /// <summary>
    ///     Удаляет указанный предмет лута из рейда.
    /// </summary>
    /// <param name="raidId">Идентификатор рейда.</param>
    /// <param name="lootId">Идентификатор предмета лута, который нужно удалить.</param>
    Task DeleteAsync(long raidId, int lootId);
}