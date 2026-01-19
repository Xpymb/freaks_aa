namespace Freaks.Portal.SharedContracts.Requests.SalarySummary.SalaryGuildLeader;

/// <summary>
///     Запрос на обновление информации о доле руководства гильдии в зарплатном периоде.
/// </summary>
/// <param name="LootId">Идентификатор предмета лута.</param>
/// <param name="Quantity">Количество предметов.</param>
/// <param name="PricePerItem">Цена за единицу лута.</param>
public record UpdateSalaryGuildLeaderRequest(
    int LootId,
    int Quantity,
    decimal PricePerItem);
