using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Freaks.Contracts.Common.Interfaces;
using Freaks.Dal.Common.Consts;

namespace Freaks.Portal.Contracts.Entities.SalarySummary;

/// <summary>
///     Итоговый отчёт зарплатного периода.
///     Содержит агрегированные данные по доходам, расходам и распределению ресурсов.
/// </summary>
[Table("salary_final_report", Schema = DatabaseConsts.PortalSchema)]
public class SalaryFinalReport : IEntity<long>
{
    /// <inheritdoc />
    [Key]
    [Column("salary_id")]
    public long Id { get; init; }

    /// <summary>
    ///     Общий доход в золоте за период.
    /// </summary>
    [Column("total_gold")]
    public decimal TotalGold { get; set; }

    /// <summary>
    ///     Общий доход в опыте синтеза инфузий мирового босса за период.
    /// </summary>
    [Column("total_world_boss_infusion")]
    public int TotalWorldBossInfusion { get; set; }

    /// <summary>
    ///     Общий доход в опыте синтеза эренорских инфузий за период.
    /// </summary>
    [Column("total_erenor_infusion")]
    public int TotalErenorInfusion { get; set; }

    /// <summary>
    ///     Золото, забранное руководством гильдии.
    /// </summary>
    [Column("gold_guild_leader_expenses")]
    public decimal GoldGuildLeaderExpenses { get; set; }

    /// <summary>
    ///     Инфузии мирового босса, забранные руководством гильдии (опыт синтеза).
    /// </summary>
    [Column("world_boss_infusion_guild_leader_expenses")]
    public int WorldBossInfusionGuildLeaderExpenses { get; set; }

    /// <summary>
    ///     Эренорские инфузии, забранные руководством гильдии (опыт синтеза).
    /// </summary>
    [Column("erenor_infusion_guild_leader_expenses")]
    public int ErenorInfusionGuildLeaderExpenses { get; set; }

    /// <summary>
    ///     Расходы гильдии в золоте.
    /// </summary>
    [Column("gold_expenses")]
    public decimal GoldExpenses { get; set; }

    /// <summary>
    ///     Расходы гильдии в инфузиях мирового босса (опыт синтеза).
    /// </summary>
    [Column("world_boss_infusion_expenses")]
    public decimal WorldBossInfusionExpenses { get; set; }

    /// <summary>
    ///     Расходы гильдии в эренорских инфузиях (опыт синтеза).
    /// </summary>
    [Column("erenor_infusion_expenses")]
    public decimal ErenorInfusionExpenses { get; set; }

    /// <summary>
    ///     Золото, распределённое участникам в качестве зарплаты.
    /// </summary>
    [Column("gold_for_salary")]
    public decimal GoldForSalary { get; set; }

    /// <summary>
    ///     Инфузии мирового босса, распределённые участникам (опыт синтеза).
    /// </summary>
    [Column("world_boss_infusion_for_salary")]
    public decimal WorldBossInfusionForSalary { get; set; }

    /// <summary>
    ///     Инфузии мирового босса, оставшиеся на продажу (опыт синтеза).
    /// </summary>
    [Column("world_boss_infusion_for_sale")]
    public decimal WorldBossInfusionForSale { get; set; }

    /// <summary>
    ///     Эренорские инфузии, распределённые участникам (опыт синтеза).
    /// </summary>
    [Column("erenor_infusion_for_salary")]
    public decimal ErenorInfusionForSalary { get; set; }

    /// <summary>
    ///     Эренорские инфузии, оставшиеся на продажу (опыт синтеза).
    /// </summary>
    [Column("erenor_infusion_for_sale")]
    public decimal ErenorInfusionForSale { get; set; }

    /// <summary>
    ///     Навигационное свойство для доступа к зарплатному периоду.
    /// </summary>
    public Salary? Salary { get; init; }
}
