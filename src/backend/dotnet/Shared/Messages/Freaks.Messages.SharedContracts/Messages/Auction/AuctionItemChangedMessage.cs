using Freaks.Messages.SharedContracts.Attributes;
using Freaks.Portal.SharedContracts.ValueObjects.Auction;

namespace Freaks.Messages.SharedContracts.Messages.Auction;

/// <summary>
///     Сообщение об изменении состояния аукционного лота,
///     содержит информацию о выполненном действии и параметры лота.
/// </summary>
[MessageTopic("auction")]
public class AuctionItemChangedMessage : BaseMessage
{
    /// <summary>
    ///     Идентификатор лота аукциона, для которого произошло изменение.
    /// </summary>
    public required long Id { get; init; }

    /// <summary>
    ///     Новый статус аукционного лота после изменения.
    /// </summary>
    public AuctionItemStatus? Status { get; init; }
}