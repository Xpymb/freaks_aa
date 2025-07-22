namespace Freaks.Portal.SharedContracts.Requests.RaidSummary.RaidScreenshot;

/// <summary>
///     Запрос на добавление скриншотов к рейду.
///     Содержит идентификатор рейда и ссылки на изображения.
/// </summary>
/// <param name="RaidId">Идентификатор рейда, к которому прикрепляется скриншот.</param>
/// <param name="ScreenshotUrls">URL-адреса загружаемых скриншотов.</param>
public record SetScreenshotRequest(
    int RaidId,
    IList<string> ScreenshotUrls);