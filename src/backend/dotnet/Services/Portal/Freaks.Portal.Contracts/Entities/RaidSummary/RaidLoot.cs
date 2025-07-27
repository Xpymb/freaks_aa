using System.ComponentModel.DataAnnotations.Schema;
using Freaks.Contracts.Common.Interfaces;
using Freaks.Dal.Common.Consts;
using Freaks.Portal.Contracts.Entities.Loot;

namespace Freaks.Portal.Contracts.Entities.RaidSummary;

/// <summary>
///     Составной ключ для сущности, представляющей добычу в рейде.
///     Объединяет идентификаторы рейда и предмета.
/// </summary>
/// <param name="RaidId">Идентификатор рейда.</param>
/// <param name="LootId">Идентификатор предмета добычи.</param>
public record RaidLootKey(long RaidId, int LootId);

/// <summary>
///     Представляет добычу, выпавшую в рамках конкретного рейда. 
///     Связывает рейд с полученным типом добычи через идентификаторы.
/// </summary>
[Table("raid_loot", Schema = DatabaseConsts.PortalSchema)]
public class RaidLoot : ICompositeEntity<RaidLootKey>
{
    /// <summary>
    ///     Идентификатор рейда, в котором была получена добыча.
    /// </summary>
    [Column("raid_id")]
    public required long RaidId { get; init; }

    /// <summary>
    ///     Идентификатор добычи, полученной в рейде.
    /// </summary>
    [Column("loot_item_id")]
    public required int LootItemId { get; init; }

    /// <summary>
    ///     Количество добычи
    /// </summary>
    [Column("quantity")]
    public required int Quantity { get; set; }

    /// <summary>
    ///     Идентификатор пользователя, который создал запись
    /// </summary>
    [Column("creator_id")]
    public required Guid CreatorId { get; init; }

    /// <summary>
    ///     Навигационное свойство для доступа к рейду, в котором была получена добыча.
    /// </summary>
    public Raid? Raid { get; init; }

    /// <summary>
    ///     Навигационное свойство для доступа к информации о предмете добычи.
    /// </summary>
    public LootItem? Loot { get; init; }

    /// <inheritdoc />
    public RaidLootKey GetCompositeKey()
    {
        return new RaidLootKey(RaidId, LootItemId);
    }
}