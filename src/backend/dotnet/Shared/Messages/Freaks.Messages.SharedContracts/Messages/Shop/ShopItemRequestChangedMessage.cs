using Freaks.Messages.SharedContracts.Attributes;

namespace Freaks.Messages.SharedContracts.Messages.Shop;

/// <summary>
///     Сообщение об изменении запроса на покупку товара в магазине,
///     содержит информацию о выполненном действии и идентификаторе товара.
/// </summary>
[MessageTopic("shop.request")]
public class ShopItemRequestChangedMessage : BaseMessage
{
    /// <summary>
    ///     Идентификатор товара, запрос на покупку которого был изменён.
    /// </summary>
    public required long ShopItemId { get; init; }
}