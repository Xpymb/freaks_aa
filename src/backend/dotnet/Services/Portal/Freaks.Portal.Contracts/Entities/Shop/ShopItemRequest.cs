using System.ComponentModel.DataAnnotations.Schema;
using Freaks.Contracts.Common.Interfaces;
using Freaks.Dal.Common.Consts;
using Freaks.Portal.SharedContracts.ValueObjects.Shop;
using Freaks.Users.Contracts;

namespace Freaks.Portal.Contracts.Entities.Shop;

/// <summary>
///     Композитный ключ для сущности <see cref="ShopItemRequest" />, содержащий идентификаторы товара и пользователя.
/// </summary>
/// <param name="ShopItemId">Идентификатор товара в магазине.</param>
/// <param name="UserId">Идентификатор пользователя, сделавшего запрос.</param>
public record ShopItemRequestKey(int ShopItemId, Guid UserId);

/// <summary>
///     Представляет заявку пользователя на покупку товара из магазина.
///     Использует составной ключ из идентификаторов товара и пользователя.
/// </summary>
[Table("shop_item_requests", Schema = DatabaseConsts.PortalSchema)]
public class ShopItemRequest : ICompositeEntity<ShopItemRequestKey>
{
    /// <summary>
    ///     Идентификатор товара магазина, на который подана заявка.
    /// </summary>
    [Column("shop_item_id")]
    public required int ShopItemId { get; init; }

    /// <summary>
    ///     Идентификатор пользователя, подавшего заявку.
    /// </summary>
    [Column("user_id")]
    public required Guid UserId { get; init; }

    /// <summary>
    ///     Количество товара.
    /// </summary>
    [Column("quantity")]
    public required int Quantity { get; init; }

    /// <summary>
    ///     Дата и время создания заявки.
    /// </summary>
    [Column("created_dt")]
    public required DateTimeOffset CreatedDt { get; init; }

    /// <summary>
    ///     Текущий статус заявки.
    /// </summary>
    [Column("status")]
    public required ShopItemRequestStatus Status { get; set; }

    /// <summary>
    ///     Навигационное свойство для товара, связанного с заявкой.
    /// </summary>
    public ShopItem? Item { get; init; }

    /// <summary>
    ///     Навигационное свойство для пользователя, подавшего заявку.
    /// </summary>
    public User? User { get; init; }

    /// <summary>
    ///     Возвращает составной ключ заявки по товару и пользователю.
    /// </summary>
    /// <returns>Объект <see cref="ShopItemRequestKey" /> с идентификаторами.</returns>
    public ShopItemRequestKey GetCompositeKey()
    {
        return new ShopItemRequestKey(ShopItemId, UserId);
    }
}