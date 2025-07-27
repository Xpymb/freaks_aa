using System.ComponentModel.DataAnnotations.Schema;
using Freaks.Contracts.Common.Interfaces;
using Freaks.Dal.Common.Consts;

namespace Freaks.Portal.Contracts.Entities.RaidSummary;

/// <summary>
///     Составной ключ для сущности <see cref="RaidScreenshot" />.
///     Определяет уникальность скриншота по идентификатору рейда и URL изображения.
/// </summary>
/// <param name="RaidId">Идентификатор рейда, к которому относится скриншот.</param>
/// <param name="ScreenshotUrl">Уникальный URL-адрес скриншота.</param>
public record RaidScreenshotKey(long RaidId, string ScreenshotUrl);

/// <summary>
///     Представляет скриншот, связанный с рейдом в базе данных.
///     Содержит информацию о том, к какому рейду относится изображение, и URL скриншота.
/// </summary>
[Table("raid_screenshot", Schema = DatabaseConsts.PortalSchema)]
public class RaidScreenshot : ICompositeEntity<RaidScreenshotKey>
{
    /// <summary>
    ///     Уникальный идентификатор рейда, к которому относится скриншот.
    /// </summary>
    [Column("id")]
    public required long RaidId { get; init; }

    /// <summary>
    ///     URL-адрес скриншота рейда.
    /// </summary>
    [Column("screenshot_url")]
    public required string ScreenshotUrl { get; init; }

    /// <summary>
    ///     Идентификатор пользователя, который создал запись
    /// </summary>
    [Column("creator_id")]
    public required Guid CreatorId { get; init; }

    /// <summary>
    ///     Навигационное свойство для доступа к рейду, к которому относится скриншот.
    /// </summary>
    public Raid? Raid { get; init; }

    /// <inheritdoc />
    public RaidScreenshotKey GetCompositeKey()
    {
        return new RaidScreenshotKey(RaidId, ScreenshotUrl);
    }
}