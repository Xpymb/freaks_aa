using Freaks.Messages.SharedContracts.Attributes;
using Freaks.Portal.SharedContracts.ValueObjects.Shop;

namespace Freaks.Messages.SharedContracts.Messages.Shop;

/// <summary>
///     Сообщение об изменении товара в магазине,
///     содержит информацию о выполненном действии и идентификаторе товара.
/// </summary>
[MessageTopic("shop")]
public class ShopItemChangedMessage : BaseMessage
{
    /// <summary>
    ///     Идентификатор товара, для которого произошло изменение.
    /// </summary>
    public required int Id { get; init; }

    /// <summary>
    ///     Новый статус товара после изменения.
    /// </summary>
    public ShopItemStatus? Status { get; init; }
}