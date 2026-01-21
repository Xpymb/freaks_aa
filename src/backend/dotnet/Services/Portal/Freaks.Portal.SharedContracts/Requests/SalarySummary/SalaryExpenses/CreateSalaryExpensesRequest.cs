using Freaks.Portal.SharedContracts.ValueObjects.SalarySummary;

namespace Freaks.Portal.SharedContracts.Requests.SalarySummary.SalaryExpenses;

/// <summary>
///     Запрос на создание новой записи о расходах гильдии в зарплатном периоде.
/// </summary>
/// <param name="ExpensesType">Тип расхода.</param>
/// <param name="UserId">Идентификатор пользователя, которому назначено поощрение.</param>
/// <param name="Percentage">Процент от общей суммы.</param>
/// <param name="Amount">Сумма расхода.</param>
public record CreateSalaryExpensesRequest(
    SalaryExpensesType ExpensesType,
    Guid? UserId,
    decimal? Percentage,
    decimal? Amount);
