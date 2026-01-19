using Freaks.Portal.SharedContracts.ValueObjects.SalarySummary;

namespace Freaks.Portal.SharedContracts.Dto.SalarySummary;

/// <summary>
///     DTO, представляющее расходы гильдии в зарплатном периоде.
/// </summary>
/// <param name="Id">Идентификатор статьи расходов.</param>
/// <param name="SalaryId">Идентификатор зарплатного периода.</param>
/// <param name="ExpensesType">Тип расхода.</param>
/// <param name="UserId">Идентификатор пользователя, которому назначено поощрение.</param>
/// <param name="Percentage">Процент от общей суммы.</param>
/// <param name="Amount">Сумма расхода.</param>
public record SalaryExpensesDto(
    long Id,
    long SalaryId,
    SalaryExpensesType ExpensesType,
    Guid? UserId,
    decimal Percentage,
    decimal Amount);
