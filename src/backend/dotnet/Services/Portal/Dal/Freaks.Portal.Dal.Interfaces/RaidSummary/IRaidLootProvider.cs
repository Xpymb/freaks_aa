using Freaks.Dal.Common.Interfaces;
using Freaks.Portal.Contracts.Entities.RaidSummary;
using Freaks.Portal.Dal.Persistence;

namespace Freaks.Portal.Dal.Interfaces.RaidSummary;

/// <summary>
/// Интерфейс провайдера для работы с добычей, полученной в рейдах.
/// Позволяет получать, создавать, обновлять и удалять информацию о луте рейдов.
/// </summary>
public interface IRaidLootProvider : IBaseCompositeProvider<RaidLoot, RaidLootKey, IPortalDbContext>
{
    /// <summary>
    /// Возвращает список добычи, полученной в указанном рейде.
    /// </summary>
    /// <param name="raidId">Идентификатор рейда.</param>
    /// <returns>Список добычи, связанной с рейдом.</returns>
    Task<IList<RaidLoot>> GetByRaidIdAsync(int raidId);
}
