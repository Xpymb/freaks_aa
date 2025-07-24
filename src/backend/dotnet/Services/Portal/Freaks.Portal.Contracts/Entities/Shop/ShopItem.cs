using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Freaks.Contracts.Common.Interfaces;
using Freaks.Dal.Common.Consts;
using Freaks.Portal.Contracts.Entities.Loot;
using Freaks.Portal.SharedContracts.ValueObjects.Shop;
using Freaks.Users.Contracts;

namespace Freaks.Portal.Contracts.Entities.Shop;

/// <summary>
///     Сущность предмета магазина, доступного для покупки.
///     Связывает предмет лута с ценой, количеством и автором (создателем).
/// </summary>
[Table("shop_items", Schema = DatabaseConsts.PortalSchema)]
public class ShopItem : IEntity<int>
{
    /// <summary>
    ///     Уникальный идентификатор записи магазина.
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; init; }

    /// <summary>
    ///     Идентификатор связанного предмета добычи (лут-объекта).
    /// </summary>
    [Column("loot_item_id")]
    public required int LootItemId { get; init; }

    /// <summary>
    ///     Идентификатор пользователя, создавшего товар (например, выставил на продажу).
    /// </summary>
    [Column("creator_id")]
    public required Guid CreatorId { get; init; }

    /// <summary>
    ///     Цена предмета в условных единицах.
    /// </summary>
    [Column("price")]
    public required int Price { get; set; }

    /// <summary>
    ///     Общее запрошенное количество товара.
    /// </summary>
    [Column("total_quantity")]
    public required int TotalQuantity { get; set; }

    /// <summary>
    ///     Оставшееся количество товара, доступное для завершения заявки.
    /// </summary>
    [Column("remaining_quantity")]
    public required int RemainingQuantity { get; set; }

    /// <summary>
    ///     Статус предмета магазина
    /// </summary>
    [Column("status")]
    public required ShopItemStatus Status { get; set; }

    /// <summary>
    ///     Навигационное свойство: связанный предмет добычи.
    /// </summary>
    public LootItem? LootItem { get; init; }

    /// <summary>
    ///     Навигационное свойство: пользователь, создавший товар.
    /// </summary>
    public User? Creator { get; init; }

    /// <summary>
    ///     Навигационное свойство: заявки на покупку товара
    /// </summary>
    public List<ShopItemRequest> Requests { get; init; } = [];
}