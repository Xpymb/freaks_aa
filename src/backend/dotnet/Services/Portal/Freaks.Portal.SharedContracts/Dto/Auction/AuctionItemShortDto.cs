using Freaks.Portal.SharedContracts.Dto.Loot;
using Freaks.Portal.SharedContracts.ValueObjects.Auction;
using Freaks.Users.SharedContracts.Dto;

namespace Freaks.Portal.SharedContracts.Dto.Auction;

/// <summary>
///     DTO для краткого представления информации о лоте аукциона.
/// </summary>
public record AuctionItemShortDto
{
    /// <summary>
    ///     Уникальный идентификатор лота.
    /// </summary>
    public long Id { get; init; }

    /// <summary>
    ///     Информация о предмете лота.
    /// </summary>
    public LootItemDto LootItem { get; init; }

    /// <summary>
    ///     Дата и время окончания аукциона по лоту.
    /// </summary>
    public DateTimeOffset EndDt { get; init; }

    /// <summary>
    ///     Текущая максимальная ставка по лоту.
    /// </summary>
    public decimal? LastBidPrice { get; set; }

    /// <summary>
    ///     Информация о пользователе, предложившем лот.
    /// </summary>
    public UserDto Creator { get; init; }

    /// <summary>
    ///     Текущий статус лота.
    /// </summary>
    public AuctionItemStatus Status { get; init; }
}