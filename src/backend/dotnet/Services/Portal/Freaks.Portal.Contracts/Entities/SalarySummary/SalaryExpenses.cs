using System.ComponentModel.DataAnnotations.Schema;
using Freaks.Contracts.Common.Interfaces;
using Freaks.Dal.Common.Consts;
using Freaks.Portal.SharedContracts.ValueObjects.SalarySummary;

namespace Freaks.Portal.Contracts.Entities.SalarySummary;

/// <summary>
///     Композитный ключ для зарплатных расходов
/// </summary>
/// <param name="SalaryId">Идентификатор зарплатного периода</param>
/// <param name="ExpensesType">Тип расхода</param>
public record SalaryExpensesKey(long SalaryId, SalaryExpensesType ExpensesType);

/// <summary>
///     Расходы и отчисления зарплатного периода
/// </summary>
[Table("salary_expenses", Schema = DatabaseConsts.PortalSchema)]
public class SalaryExpenses : ICompositeEntity<SalaryExpensesKey>
{
    /// <summary>
    ///     Идентификатор зарплатного периода
    /// </summary>
    [Column("salary_id")]
    public required long SalaryId { get; init; }

    /// <summary>
    ///     Тип расхода
    /// </summary>
    [Column("expenses_type")]
    public required SalaryExpensesType ExpensesType { get; init; }

    /// <summary>
    ///     Процент от общей суммы
    /// </summary>
    [Column("percentage")]
    public required decimal Percentage { get; set; }

    /// <summary>
    ///     Сумма расхода
    /// </summary>
    [Column("amount")]
    public required decimal Amount { get; set; }

    /// <summary>
    ///     Навигационное свойство для доступа к информации о зарплатном периоде.
    /// </summary>
    public Salary? Salary { get; init; }

    /// <inheritdoc />
    public SalaryExpensesKey GetCompositeKey()
    {
        return new SalaryExpensesKey(SalaryId, ExpensesType);
    }
}