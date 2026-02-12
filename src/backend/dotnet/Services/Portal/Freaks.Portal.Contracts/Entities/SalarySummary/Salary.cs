using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Freaks.Contracts.Common.Interfaces;
using Freaks.Dal.Common.Consts;
using Freaks.Portal.SharedContracts.ValueObjects.RaidSummary;
using Freaks.Portal.SharedContracts.ValueObjects.SalarySummary;

namespace Freaks.Portal.Contracts.Entities.SalarySummary;

/// <summary>
///     Зарплатный период
/// </summary>
[Table("salary", Schema = DatabaseConsts.PortalSchema)]
public class Salary : IEntity<long>
{
    /// <inheritdoc />
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public long Id { get; init; }

    /// <summary>
    ///     Название зарплатного периода
    /// </summary>
    [Column("name")]
    public required string Name { get; set; }

    /// <summary>
    ///     Дата начала периода
    /// </summary>
    [Column("start_dt")]
    public required DateOnly StartDt { get; set; }

    /// <summary>
    ///     Дата окончания периода
    /// </summary>
    [Column("end_dt")]
    public required DateOnly EndDt { get; set; }

    /// <summary>
    ///     Статус заполнения зарплаты
    /// </summary>
    [Column("fill_status")]
    public required SalaryFillStepType FillStepType { get; set; }

    /// <summary>
    ///     Статус регистрации на зарплату
    /// </summary>
    [Column("registration_status")]
    public required SalaryRegistrationStatus RegistrationStatus { get; set; }

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
    ///     Создатель зарплатного периода
    /// </summary>
    [Column("created_by")]
    public required Guid CreatedBy { get; init; }

    /// <summary>
    ///     Дата создания
    /// </summary>
    [Column("created_dt")]
    public required DateTime CreatedDt { get; init; }

    /// <summary>
    ///     Дата обновления
    /// </summary>
    [Column("updated_dt")]
    public DateTime? UpdatedDt { get; set; }

    /// <summary>
    ///     Признак: зарплатный период завершён
    /// </summary>
    [Column("is_finished")]
    public bool IsFinished { get; set; }
}