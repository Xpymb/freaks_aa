using Freaks.Dal.Common.ValueObjects;
using Freaks.Portal.Contracts.Entities.RaidSummary;

namespace Freaks.Portal.Dal.Interfaces.RaidSummary;

/// <summary>
/// Интерфейс провайдера для работы с добычей, полученной в рейдах.
/// Позволяет получать, создавать, обновлять и удалять информацию о луте рейдов.
/// </summary>
public interface IRaidLootProvider
{
    /// <summary>
    /// Возвращает объект лута, связанный с рейдом и конкретным предметом.
    /// </summary>
    /// <param name="raidId">Идентификатор рейда.</param>
    /// <param name="lootId">Идентификатор предмета лута.</param>
    /// <param name="trackingType">Режим отслеживания изменений (tracking или no-tracking).</param>
    /// <returns>Объект лута или null, если не найден.</returns>
    Task<RaidLoot?> GetAsync(int raidId, int lootId, EntityTrackingType trackingType);

    /// <summary>
    /// Возвращает список добычи, полученной в указанном рейде.
    /// </summary>
    /// <param name="raidId">Идентификатор рейда.</param>
    /// <returns>Список добычи, связанной с рейдом.</returns>
    Task<IList<RaidLoot>> GetByRaidIdAsync(int raidId);

    /// <summary>
    /// Создаёт запись о добыче, полученной в рейде.
    /// </summary>
    /// <param name="loot">Объект лута для создания.</param>
    /// <returns>Созданный объект лута.</returns>
    Task<RaidLoot> CreateAsync(RaidLoot loot);

    /// <summary>
    /// Обновляет запись о луте в базе данных.
    /// </summary>
    /// <param name="loot">Обновлённый объект лута.</param>
    /// <returns>Обновлённая сущность лута.</returns>
    Task<RaidLoot> UpdateAsync(RaidLoot loot);

    /// <summary>
    /// Удаляет запись о добыче из рейда.
    /// </summary>
    /// <param name="loot">Объект лута, который необходимо удалить.</param>
    Task DeleteAsync(RaidLoot loot);
}
