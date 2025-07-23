namespace Freaks.Portal.SharedContracts.Requests.RaidSummary.RaidLoot;

/// <summary>
///     Запрос на добавление лута, полученного в рейде.
///     Содержит идентификатор рейда, идентификатор предмета и количество.
/// </summary>
/// <param name="LootId">Идентификатор предмета лута (из справочника).</param>
/// <param name="Amount">Количество предметов, полученных в рейде.</param>
public record CreateRaidLootRequest(
    int LootId,
    int Amount);