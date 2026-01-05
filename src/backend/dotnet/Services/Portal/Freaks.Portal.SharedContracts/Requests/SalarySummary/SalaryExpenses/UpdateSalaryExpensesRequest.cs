namespace Freaks.Portal.SharedContracts.Requests.SalarySummary.SalaryExpenses;

/// <summary>
///     Запрос на обновление информации о расходах гильдии в зарплатном периоде.
/// </summary>
/// <param name="Percentage">Процент от общей суммы.</param>
/// <param name="Amount">Сумма расхода.</param>
public record UpdateSalaryExpensesRequest(
    decimal Percentage,
    decimal Amount);
