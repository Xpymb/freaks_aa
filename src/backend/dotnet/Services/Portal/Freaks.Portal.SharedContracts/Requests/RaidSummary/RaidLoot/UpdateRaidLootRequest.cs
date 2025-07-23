namespace Freaks.Portal.SharedContracts.Requests.RaidSummary.RaidLoot;

/// <summary>
///     Запрос на обновление количества предметов лута, связанных с рейдом.
///     Используется для изменения количества уже добавленного лута.
/// </summary>
/// <param name="Amount">Новое количество предметов.</param>
public record UpdateRaidLootRequest(
    int Amount);