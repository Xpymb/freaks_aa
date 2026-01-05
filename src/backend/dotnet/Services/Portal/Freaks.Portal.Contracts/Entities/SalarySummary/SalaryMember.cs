using System.ComponentModel.DataAnnotations.Schema;
using Freaks.Contracts.Common.Interfaces;
using Freaks.Dal.Common.Consts;
using Freaks.Portal.SharedContracts.ValueObjects.SalarySummary;
using Freaks.Users.Contracts.Entities;

namespace Freaks.Portal.Contracts.Entities.SalarySummary;

/// <summary>
///     Композитный ключ участника зарплатного периода
/// </summary>
/// <param name="SalaryId">Идентификатор зарплатного периода</param>
/// <param name="UserId">Идентификатор пользователя</param>
public record SalaryMemberKey(long SalaryId, Guid UserId);

/// <summary>
///     Участник зарплатного периода
/// </summary>
[Table("salary_member", Schema = DatabaseConsts.PortalSchema)]
public class SalaryMember : ICompositeEntity<SalaryMemberKey>
{
    /// <summary>
    ///     Идентификатор зарплатного периода
    /// </summary>
    [Column("salary_id")]
    public required long SalaryId { get; init; }

    /// <summary>
    ///     Идентификатор пользователя
    /// </summary>
    [Column("user_id")]
    public required Guid UserId { get; init; }

    /// <summary>
    ///     Тип выплаты
    /// </summary>
    [Column("payment_type")]
    public required SalaryPaymentType PaymentType { get; set; }

    /// <summary>
    ///     Процент активности
    /// </summary>
    [Column("activity_percentage")]
    public required decimal ActivityPercentage { get; set; }

    /// <summary>
    ///     Коэффициент
    /// </summary>
    [Column("coefficient")]
    public decimal? Coefficient { get; set; }

    /// <summary>
    ///     Золото за активность
    /// </summary>
    [Column("activity_gold")]
    public decimal? ActivityGold { get; set; }

    /// <summary>
    ///     Золото за ответственность
    /// </summary>
    [Column("responsibility_gold")]
    public decimal? ResponsibilityGold { get; set; }

    /// <summary>
    ///     Общая сумма золота
    /// </summary>
    [Column("amount_gold")]
    public decimal? AmountGold { get; set; }

    /// <summary>
    ///     Количество инфузий мирового босса
    /// </summary>
    [Column("amount_world_boss_infusion")]
    public decimal? AmountWorldBossInfusion { get; set; }

    /// <summary>
    ///     Навигационное свойство для доступа к информации о зарплатном периоде.
    /// </summary>
    public Salary? Salary { get; init; }

    /// <summary>
    ///     Навигационное свойство для доступа к информации о пользователе.
    /// </summary>
    public User? User { get; init; }

    /// <inheritdoc />
    public SalaryMemberKey GetCompositeKey()
    {
        return new SalaryMemberKey(SalaryId, UserId);
    }
}