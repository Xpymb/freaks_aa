using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Freaks.Contracts.Common.Interfaces;
using Freaks.Dal.Common.Consts;
using Freaks.Portal.SharedContracts.ValueObjects.SalarySummary;

namespace Freaks.Portal.Contracts.Entities.SalarySummary;

/// <summary>
///     Расходы и отчисления зарплатного периода
/// </summary>
[Table("salary_expenses", Schema = DatabaseConsts.PortalSchema)]
public class SalaryExpenses : IEntity<long>
{
    /// <inheritdoc />
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public long Id { get; init; }

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
    ///     Идентификатор пользователя, которому назначено поощрение
    /// </summary>
    [Column("user_id")]
    public Guid? UserId { get; init; }

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
}