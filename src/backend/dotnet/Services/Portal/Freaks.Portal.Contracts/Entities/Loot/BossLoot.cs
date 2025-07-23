using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Freaks.Contracts.Common.Interfaces;
using Freaks.Dal.Common.Consts;
using Freaks.Portal.SharedContracts.ValueObjects.Loot;

namespace Freaks.Portal.Contracts.Entities.Loot;

/// <summary>
/// Представляет возможную добычу от мирового босса.
/// Содержит информацию о типе и ранге добычи, названии предмета и опыте инфузии.
/// </summary>
[Table("boss_loot", Schema = DatabaseConsts.PortalSchema)]
public class BossLoot : IEntity<int>
{
    /// <summary>
    /// Уникальный идентификатор записи о добыче.
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; init; }

    /// <summary>
    /// Тип добычи (эссенция, кристалл, экипировка и т.д.).
    /// </summary>
    [Column("loot_type")]
    public required LootType Type { get; init; }

    /// <summary>
    /// Ранг (уровень редкости) добычи.
    /// </summary>
    [Column("grade_type")]
    public required LootGradeType GradeType { get; init; }

    /// <summary>
    /// Название предмета добычи.
    /// </summary>
    [Column("item_name")]
    public required string Name { get; init; }

    /// <summary>
    ///     Описание предмета добычи.
    /// </summary>
    [Column("item_description")]
    public required string Description { get; init; }

    /// <summary>
    /// Опыт синтеза, получаемый при использовании эссенции, или null для предметов без опыта синтеза.
    /// </summary>
    [Column("synthesis_exp")]
    public required int? SynthesisExp { get; init; }
}