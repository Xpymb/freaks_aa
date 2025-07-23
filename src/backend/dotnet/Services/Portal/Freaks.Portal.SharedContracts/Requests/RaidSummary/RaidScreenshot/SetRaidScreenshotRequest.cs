namespace Freaks.Portal.SharedContracts.Requests.RaidSummary.RaidScreenshot;

/// <summary>
///     Запрос на добавление скриншотов к рейду.
///     Содержит идентификатор рейда и ссылки на изображения.
/// </summary>
/// <param name="ScreenshotUrls">URL-адреса загружаемых скриншотов.</param>
public record SetRaidScreenshotRequest(
    IList<string> ScreenshotUrls);