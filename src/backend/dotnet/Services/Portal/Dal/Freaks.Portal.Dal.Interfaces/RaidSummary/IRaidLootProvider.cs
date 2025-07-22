using Freaks.Portal.Contracts.Entities.RaidSummary;

namespace Freaks.Portal.Dal.Interfaces.RaidSummary;

/// <summary>
/// Интерфейс провайдера для работы с добычей, полученной в рейдах.
/// Позволяет получать, создавать, обновлять и удалять информацию о луте рейдов.
/// </summary>
public interface IRaidLootProvider
{
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
    /// Удаляет запись о добыче из рейда.
    /// </summary>
    /// <param name="loot">Объект лута, который необходимо удалить.</param>
    Task DeleteAsync(RaidLoot loot);
}