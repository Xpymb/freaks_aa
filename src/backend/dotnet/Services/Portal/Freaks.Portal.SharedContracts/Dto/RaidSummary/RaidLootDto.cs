using Freaks.Portal.SharedContracts.Dto.Loot;

namespace Freaks.Portal.SharedContracts.Dto.RaidSummary;

/// <summary>
///     DTO, представляющее предмет лута, полученный в результате рейда.
///     Связывает рейд с конкретным предметом, выпавшим с босса.
/// </summary>
/// <param name="RaidId">Идентификатор рейда, в котором был получен лут.</param>
/// <param name="BossLoot">Информация о предмете лута, полученном в рейде.</param>
/// <param name="Amount">Количество предметов, полученных в рейде.</param>
public record RaidLootDto(
    int RaidId,
    BossLootDto BossLoot,
    int Amount);