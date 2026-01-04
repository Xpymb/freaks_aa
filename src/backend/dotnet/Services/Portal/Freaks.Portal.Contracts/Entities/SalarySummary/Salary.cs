using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Freaks.Contracts.Common.Interfaces;
using Freaks.Dal.Common.Consts;
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
    public required DateOnly StartDt { get; init; }

    /// <summary>
    ///     Дата окончания периода
    /// </summary>
    [Column("end_dt")]
    public required DateOnly EndDt { get; init; }

    /// <summary>
    ///     Статус заполнения зарплаты
    /// </summary>
    [Column("fill_status")]
    public required SalaryFillStatus FillStatus { get; set; }

    /// <summary>
    ///     Статус регистрации на зарплату
    /// </summary>
    [Column("registration_status")]
    public required SalaryRegistrationStatus RegistrationStatus { get; set; }
}