namespace Freaks.Portal.SharedContracts.Requests.SalarySummary.Salary;

/// <summary>
///     Запрос на создание нового зарплатного периода.
///     Содержит базовую информацию, необходимую для создания: название и даты начала/окончания.
/// </summary>
/// <param name="Name">Название зарплатного периода.</param>
/// <param name="StartDt">Дата начала периода.</param>
/// <param name="EndDt">Дата окончания периода.</param>
public record CreateSalaryRequest(
    string Name,
    DateOnly StartDt,
    DateOnly EndDt);
