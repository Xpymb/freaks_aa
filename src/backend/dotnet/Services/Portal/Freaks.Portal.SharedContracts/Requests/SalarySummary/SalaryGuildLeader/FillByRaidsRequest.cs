namespace Freaks.Portal.SharedContracts.Requests.SalarySummary.SalaryGuildLeader;

/// <summary>
///     Запрос на автоматическое заполнение проданного лута зарплатного периода на основе данных из рейдов.
/// </summary>
/// <param name="LootIds">Список идентификаторов предметов лута для автоматического заполнения.</param>
public record FillByRaidsRequest(
    IList<int> LootIds);