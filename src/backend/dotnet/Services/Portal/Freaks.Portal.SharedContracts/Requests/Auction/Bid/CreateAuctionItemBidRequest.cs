namespace Freaks.Portal.SharedContracts.Requests.Auction.Bid;

/// <summary>
///     Запрос на создание ставки на аукционе
/// </summary>
/// <param name="Price">Цена ставки</param>
public record CreateAuctionItemBidRequest(decimal Price);