namespace Freaks.Portal.SharedContracts.Requests.RaidSummary.RaidLoot;

/// <summary>
///     Запрос на обновление количества предметов лута, связанных с рейдом.
///     Используется для изменения количества уже добавленного лута.
/// </summary>
/// <param name="RaidId">Идентификатор рейда, в котором нужно обновить лут.</param>
/// <param name="LootId">Идентификатор предмета лута, который нужно обновить.</param>
/// <param name="Amount">Новое количество предметов.</param>
public record UpdateRaidLootRequest(
    int RaidId,
    int LootId,
    int Amount);