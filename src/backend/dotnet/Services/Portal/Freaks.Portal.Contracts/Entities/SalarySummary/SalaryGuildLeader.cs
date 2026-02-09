using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Freaks.Contracts.Common.Interfaces;
using Freaks.Dal.Common.Consts;
using Freaks.Portal.Contracts.Entities.Loot;

namespace Freaks.Portal.Contracts.Entities.SalarySummary;

/// <summary>
///     Доля руководства гильдии в зарплате
/// </summary>
[Table("salary_guild_leader", Schema = DatabaseConsts.PortalSchema)]
public class SalaryGuildLeader : IEntity<long>
{
    /// <inheritdoc />
    [Key]
    [Column("salary_loot_id")]
    public required long Id { get; init; }

    /// <summary>
    ///     Идентификатор зарплатного периода
    /// </summary>
    [Column("salary_id")]
    public required long SalaryId { get; init; }

    /// <summary>
    ///     Количество
    /// </summary>
    [Column("quantity")]
    public required int Quantity { get; set; }

    /// <summary>
    ///     Навигационное свойство для доступа к информации о зарплатном периоде.
    /// </summary>
    public Salary? Salary { get; init; }

    /// <summary>
    ///     Навигационное свойство для доступа к информации о предмете добычи.
    /// </summary>
    public SalaryLoot? SalaryLoot { get; init; }
}