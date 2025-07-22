using Freaks.Portal.SharedContracts.Dto.RaidSummary;
using Freaks.Portal.SharedContracts.Requests.RaidSummary.RaidScreenshot;

namespace Freaks.Portal.Bll.Interfaces.RaidSummary;

/// <summary>
///     Сервис для управления скриншотами, прикреплёнными к рейдам.
///     Предоставляет методы для получения, добавления и удаления скриншотов.
/// </summary>
public interface IRaidScreenshotService
{
    /// <summary>
    ///     Возвращает список скриншотов, прикреплённых к указанному рейду.
    /// </summary>
    /// <param name="raidId">Идентификатор рейда.</param>
    /// <returns>Список скриншотов рейда.</returns>
    Task<IList<RaidScreenshotDto>> GetListAsync(int raidId);

    /// <summary>
    ///     Добавляет новые скриншоты к рейду.
    /// </summary>
    /// <param name="request">Запрос с данными скриншота и рейда.</param>
    /// <returns>Добавленный скриншот в виде DTO.</returns>
    Task<IList<RaidScreenshotDto>> SetAsync(SetScreenshotRequest request);

    /// <summary>
    ///     Удаляет скриншот из рейда по идентификатору рейда и URL скриншота.
    /// </summary>
    /// <param name="raidId">Идентификатор рейда.</param>
    /// <param name="screenshotUrl">URL удаляемого скриншота.</param>
    Task DeleteAsync(int raidId, string screenshotUrl);
}