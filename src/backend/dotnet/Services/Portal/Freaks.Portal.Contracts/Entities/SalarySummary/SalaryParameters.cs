using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Freaks.Contracts.Common.Interfaces;
using Freaks.Dal.Common.Consts;
using Freaks.Portal.SharedContracts.ValueObjects.RaidSummary;
using Freaks.Portal.SharedContracts.ValueObjects.SalarySummary;

namespace Freaks.Portal.Contracts.Entities.SalarySummary;

/// <summary>
///     Параметры зарплатного периода
/// </summary>
[Table("salary_parameters", Schema = DatabaseConsts.PortalSchema)]
public class SalaryParameters : IEntity<long>
{
    /// <inheritdoc />
    [Key]
    [Column("salary_id")]
    public long Id { get; init; }

    /// <summary>
    ///     Разрешенные типы выплат зарплаты
    /// </summary>
    [Column("allowed_payment_types")]
    public required IList<SalaryPaymentType> AllowedPaymentTypes { get; set; }

    /// <summary>
    ///     Использовать коэффициенты при расчете
    /// </summary>
    [Column("use_coefficients")]
    public required bool UseCoefficients { get; set; }

    /// <summary>
    ///     Типы боссов для расчета зарплаты
    /// </summary>
    [Column("boss_types")]
    public required IList<BossType> BossTypes { get; set; }

    /// <summary>
    ///     Навигационное свойство для доступа к информации о зарплатном периоде.
    /// </summary>
    public Salary? Salary { get; init; }
}