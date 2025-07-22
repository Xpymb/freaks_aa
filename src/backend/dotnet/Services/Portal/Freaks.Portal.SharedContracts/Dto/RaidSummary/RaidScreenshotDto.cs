namespace Freaks.Portal.SharedContracts.Dto.RaidSummary;

/// <summary>
///     Представляет DTO скриншота, прикреплённого к рейду.
///     Содержит идентификатор рейда и ссылку на изображение.
/// </summary>
/// <param name="RaidId">Идентификатор рейда, к которому относится скриншот.</param>
/// <param name="ScreenshotUrl">URL-адрес скриншота, загруженного после рейда.</param>
public record RaidScreenshotDto(
    int RaidId,
    string ScreenshotUrl);