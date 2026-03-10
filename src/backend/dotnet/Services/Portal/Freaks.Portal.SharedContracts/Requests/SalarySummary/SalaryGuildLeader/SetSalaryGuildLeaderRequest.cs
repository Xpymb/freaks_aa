namespace Freaks.Portal.SharedContracts.Requests.SalarySummary.SalaryGuildLeader;

/// <summary>
///     Запрос для массовой установки долей руководства гильдии в зарплатном периоде.
///     Заменяет все существующие записи на переданный список.
/// </summary>
/// <param name="SalaryGuildLeaders">Список долей руководства гильдии для установки.</param>
public record SetSalaryGuildLeaderRequest(
    IList<CreateSalaryGuildLeaderRequest> SalaryGuildLeaders);
