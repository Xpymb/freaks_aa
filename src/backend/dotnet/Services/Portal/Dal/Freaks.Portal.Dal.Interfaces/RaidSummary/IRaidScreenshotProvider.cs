using Freaks.Dal.Common.Interfaces;
using Freaks.Portal.Contracts.Entities.RaidSummary;
using Freaks.Portal.Dal.Persistence;

namespace Freaks.Portal.Dal.Interfaces.RaidSummary;

/// <summary>
/// Интерфейс провайдера для работы со скриншотами, связанными с рейдами.
/// Позволяет получать, сохранять и удалять изображения, прикреплённые к рейдам.
/// </summary>
public interface IRaidScreenshotProvider : IBaseCompositeProvider<RaidScreenshot, RaidScreenshotKey, IPortalDbContext>
{
    /// <summary>
    /// Возвращает список скриншотов, прикреплённых к указанному рейду.
    /// </summary>
    /// <param name="raidId">Идентификатор рейда.</param>
    /// <returns>Список скриншотов, связанных с рейдом.</returns>
    Task<IList<RaidScreenshot>> GetByRaidIdAsync(int raidId);
}