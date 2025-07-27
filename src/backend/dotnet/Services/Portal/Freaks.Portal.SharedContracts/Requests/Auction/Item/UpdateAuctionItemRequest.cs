namespace Freaks.Portal.SharedContracts.Requests.Auction.Item;

/// <summary>
/// Запрос на обновление параметров существующего лота аукциона.
/// </summary>
/// <param name="MinPriceStep">Новый минимальный шаг повышения ставки.</param>
/// <param name="Description">Новое текстовое описание лота.</param>
public record UpdateAuctionItemRequest(
    decimal MinPriceStep,
    string Description);