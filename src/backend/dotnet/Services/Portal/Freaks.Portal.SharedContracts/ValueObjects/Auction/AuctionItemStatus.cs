namespace Freaks.Portal.SharedContracts.ValueObjects.Auction;

/// <summary>
///     Перечисление возможных статусов лота на аукционе.
/// </summary>
public enum AuctionItemStatus
{
    /// <summary>
    ///     Аукцион запущен и приём ставок активен.
    /// </summary>
    Started = 1,

    /// <summary>
    ///     Аукцион завершён, приём ставок прекращён.
    /// </summary>
    Ended = 2,
}