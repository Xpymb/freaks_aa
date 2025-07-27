namespace Freaks.Portal.SharedContracts.Requests.Auction.Item;

/// <summary>
///     Запрос на создание нового лота аукциона.
/// </summary>
/// <param name="LootItemId">Идентификатор предмета (LootItem), выставляемого на аукцион.</param>
/// <param name="StartPrice">Начальная цена лота.</param>
/// <param name="MinPriceStep">Минимальный шаг повышения ставки.</param>
/// <param name="EndDt">Дата и время окончания аукциона для данного лота.</param>
/// <param name="Description">Текстовое описание лота.</param>
public record CreateAuctionItemRequest(
    int LootItemId,
    decimal StartPrice,
    decimal MinPriceStep,
    DateTimeOffset EndDt,
    string Description);