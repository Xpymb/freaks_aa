namespace Freaks.Portal.SharedContracts.Requests.SalarySummary.SalaryGuildLeader;

/// <summary>
///     Запрос на создание новой записи о доле руководства гильдии в зарплатном периоде.
/// </summary>
/// <param name="LootId">Идентификатор предмета лута.</param>
/// <param name="Quantity">Количество предметов.</param>
/// <param name="PricePerItem">Цена за единицу лута.</param>
public record CreateSalaryGuildLeaderRequest(
    int LootId,
    int Quantity,
    decimal PricePerItem);
