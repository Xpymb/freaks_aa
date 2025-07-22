using Freaks.Portal.Contracts.Entities.RaidSummary;

namespace Freaks.Portal.Dal.Interfaces.RaidSummary;

/// <summary>
/// Интерфейс провайдера для работы со скриншотами, связанными с рейдами.
/// Позволяет получать, сохранять и удалять изображения, прикреплённые к рейдам.
/// </summary>
public interface IRaidScreenshotProvider
{
    /// <summary>
    /// Возвращает список скриншотов, прикреплённых к указанному рейду.
    /// </summary>
    /// <param name="raidId">Идентификатор рейда.</param>
    /// <returns>Список скриншотов, связанных с рейдом.</returns>
    Task<IList<RaidScreenshot>> GetByRaidIdAsync(int raidId);

    /// <summary>
    /// Заменяет список скриншотов для рейда.
    /// Старые скриншоты удаляются, новые сохраняются.
    /// </summary>
    /// <param name="screenshots">Список новых скриншотов для рейда.</param>
    /// <returns>Последний добавленный или обновлённый скриншот.</returns>
    Task<IList<RaidScreenshot>> SetAsync(IList<RaidScreenshot> screenshots);

    /// <summary>
    /// Удаляет указанный скриншот рейда.
    /// </summary>
    /// <param name="screenshot">Скриншот, подлежащий удалению.</param>
    Task DeleteAsync(RaidScreenshot screenshot);
}