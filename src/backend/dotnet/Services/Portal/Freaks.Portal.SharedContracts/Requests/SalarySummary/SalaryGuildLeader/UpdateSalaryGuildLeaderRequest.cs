namespace Freaks.Portal.SharedContracts.Requests.SalarySummary.SalaryGuildLeader;

/// <summary>
///     Запрос на обновление информации о доле руководства гильдии в зарплатном периоде.
/// </summary>
/// <param name="Quantity">Количество предметов.</param>
public record UpdateSalaryGuildLeaderRequest(
    int Quantity);
