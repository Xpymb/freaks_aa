namespace Freaks.Portal.SharedContracts.Requests.SalarySummary.SalaryGuildLeader;

/// <summary>
///     Запрос на создание новой записи о доле руководства гильдии в зарплатном периоде.
/// </summary>
/// <param name="SalaryLootId">Идентификатор предмета добытого лута в зарплатном периоде.</param>
/// <param name="Quantity">Количество предметов.</param>
public record CreateSalaryGuildLeaderRequest(
    long SalaryLootId,
    int Quantity);
