using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Freaks.Contracts.Common.Interfaces;
using Freaks.Dal.Common.Consts;
using Freaks.Portal.Contracts.Entities.Loot;

namespace Freaks.Portal.Contracts.Entities.SalarySummary;

/// <summary>
///     Проданный лут за зарплатный период
/// </summary>
[Table("salary_loot", Schema = DatabaseConsts.PortalSchema)]
public class SalaryLoot : IEntity<long>
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
    ///     Идентификатор лута
    /// </summary>
    [Column("loot_id")]
    public required int LootId { get; set; }

    /// <summary>
    ///     Количество
    /// </summary>
    [Column("quantity")]
    public required int Quantity { get; set; }

    /// <summary>
    ///     Цена за единицу лута
    /// </summary>
    [Column("price_per_loot")]
    public required decimal PricePerLoot { get; set; }

    /// <summary>
    ///     Скидка в процентах
    /// </summary>
    [Column("discount_percent")]
    public required decimal DiscountPercent { get; set; }

    /// <summary>
    ///     Итоговая сумма
    /// </summary>
    [Column("amount")]
    public required decimal Amount { get; set; }

    /// <summary>
    ///     Навигационное свойство для доступа к информации о зарплатном периоде.
    /// </summary>
    public Salary? Salary { get; init; }

    /// <summary>
    ///     Навигационное свойство для доступа к информации о предмете добычи.
    /// </summary>
    public LootItem? LootItem { get; init; }
}