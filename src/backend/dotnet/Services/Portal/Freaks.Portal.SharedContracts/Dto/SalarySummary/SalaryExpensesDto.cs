using Freaks.Portal.SharedContracts.ValueObjects.SalarySummary;

namespace Freaks.Portal.SharedContracts.Dto.SalarySummary;

/// <summary>
///     DTO, представляющее расходы гильдии в зарплатном периоде.
/// </summary>
/// <param name="SalaryId">Идентификатор зарплатного периода.</param>
/// <param name="ExpensesType">Тип расхода.</param>
/// <param name="Percentage">Процент от общей суммы.</param>
/// <param name="Amount">Сумма расхода.</param>
public record SalaryExpensesDto(
    long SalaryId,
    SalaryExpensesType ExpensesType,
    decimal Percentage,
    decimal Amount);
