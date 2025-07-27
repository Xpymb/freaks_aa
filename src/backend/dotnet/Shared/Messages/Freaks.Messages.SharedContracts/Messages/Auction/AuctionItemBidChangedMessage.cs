using Freaks.Messages.SharedContracts.Attributes;

namespace Freaks.Messages.SharedContracts.Messages.Auction;

/// <summary>
///     Сообщение об изменении ставки по лоту аукциона,
///     содержит информацию об идентификаторе лота и самой ставки.
/// </summary>
[MessageTopic("auction.bid")]
public class AuctionItemBidChangedMessage : BaseMessage
{
    /// <summary>
    ///     Идентификатор лота аукциона, для которого изменена ставка.
    /// </summary>
    public required long AuctionItemId { get; init; }

    /// <summary>
    ///     Идентификатор ставки, которая была изменена или добавлена.
    /// </summary>
    public required long BidId { get; init; }
}