using System.ComponentModel.DataAnnotations.Schema;
using Freaks.Dal.Common.Consts;
using Freaks.Portal.Contracts.Entities.Loot;

namespace Freaks.Portal.Contracts.Entities.RaidSummary;

/// <summary>
/// Представляет добычу, выпавшую в рамках конкретного рейда.
/// Связывает рейд с полученным типом добычи через идентификаторы.
/// </summary>
[Table("raid_loot", Schema = DatabaseConsts.PortalSchema)]
public class RaidLoot
{
    /// <summary>
    /// Идентификатор рейда, в котором была получена добыча.
    /// </summary>
    [Column("raid_id")]
    public required int RaidId { get; init; }

    /// <summary>
    /// Идентификатор добычи, полученной в рейде.
    /// </summary>
    [Column("loot_id")]
    public required int LootId { get; init; }

    /// <summary>
    /// Количество добычи
    /// </summary>
    [Column("amount")]
    public required int Amount { get; set; }

    /// <summary>
    ///     Идентификатор пользователя, который создал запись
    /// </summary>
    [Column("creator_id")]
    public required Guid CreatorId { get; init; }

    /// <summary>
    /// Навигационное свойство для доступа к рейду, в котором была получена добыча.
    /// </summary>
    public Raid? Raid { get; init; }

    /// <summary>
    /// Навигационное свойство для доступа к информации о предмете добычи.
    /// </summary>
    public BossLoot? Loot { get; init; }
}