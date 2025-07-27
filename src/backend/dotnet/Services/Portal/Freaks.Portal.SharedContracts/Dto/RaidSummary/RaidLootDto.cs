using Freaks.Portal.SharedContracts.Dto.Loot;

namespace Freaks.Portal.SharedContracts.Dto.RaidSummary;

/// <summary>
///     DTO, представляющее предмет лута, полученный в результате рейда.
///     Связывает рейд с конкретным предметом, выпавшим с босса.
/// </summary>
/// <param name="RaidId">Идентификатор рейда, в котором был получен лут.</param>
/// <param name="LootItem">Информация о предмете лута, полученном в рейде.</param>
/// <param name="Quantity">Количество предметов, полученных в рейде.</param>
public record RaidLootDto(
    long RaidId,
    LootItemDto LootItem,
    int Quantity);