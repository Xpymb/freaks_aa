namespace Freaks.Portal.SharedContracts.ValueObjects.Auction;

/// <summary>
///     Параметры сортировки списка лотов на аукционе.
/// </summary>
public enum AuctionItemListSortByType
{
    /// <summary>
    ///     Сортировка по дате и времени начала аукциона.
    /// </summary>
    StartDt = 0,

    /// <summary>
    ///     Сортировка по дате и времени окончания аукциона.
    /// </summary>
    EndDt = 1,
}