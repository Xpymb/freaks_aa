using System.ComponentModel.DataAnnotations.Schema;
using Freaks.Dal.Common.Consts;
using Freaks.Portal.Contracts.Entities.BossLootEntities;

namespace Freaks.Portal.Contracts.Entities.RaidEntities;

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
    
    public Raid? Raid { get; init; }
    
    public BossLoot? Loot { get; init; }
}