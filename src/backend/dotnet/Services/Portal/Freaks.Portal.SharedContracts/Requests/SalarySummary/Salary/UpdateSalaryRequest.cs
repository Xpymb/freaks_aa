namespace Freaks.Portal.SharedContracts.Requests.SalarySummary.Salary;

/// <summary>
///     Запрос на обновление информации о зарплатном периоде.
///     Содержит новое значение для названия.
/// </summary>
/// <param name="Name">Название зарплатного периода.</param>
/// <param name="From">Начало зарплатного периода.</param>
/// <param name="To">Конец зарплатного периода.</param>
public record UpdateSalaryRequest(
    string Name,
    DateOnly From,
    DateOnly To);
