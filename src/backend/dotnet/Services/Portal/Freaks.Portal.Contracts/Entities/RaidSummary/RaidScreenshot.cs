using System.ComponentModel.DataAnnotations.Schema;
using Freaks.Dal.Common.Consts;

namespace Freaks.Portal.Contracts.Entities.RaidSummary;

/// <summary>
/// Представляет скриншот, связанный с рейдом в базе данных.
/// Содержит информацию о том, к какому рейду относится изображение, и URL скриншота.
/// </summary>
[Table("raid_screenshot", Schema = DatabaseConsts.PortalSchema)]
public class RaidScreenshot
{
    /// <summary>
    /// Уникальный идентификатор рейда, к которому относится скриншот.
    /// </summary>
    [Column("id")]
    public required int RaidId { get; init; }

    /// <summary>
    /// URL-адрес скриншота рейда.
    /// </summary>
    [Column("screenshot_url")]
    public required string ScreenshotUrl { get; init; }

    /// <summary>
    ///     Идентификатор пользователя, который создал запись
    /// </summary>
    [Column("creator_id")]
    public required Guid CreatorId { get; init; }

    /// <summary>
    /// Навигационное свойство для доступа к рейду, к которому относится скриншот.
    /// </summary>
    public Raid? Raid { get; init; }
}