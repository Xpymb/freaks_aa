using Freaks.Portal.SharedContracts.Dto.Loot;
using Freaks.Portal.SharedContracts.ValueObjects.Auction;
using Freaks.Users.SharedContracts.Dto;

namespace Freaks.Portal.SharedContracts.Dto.Auction;

/// <summary>
///     DTO для полного представления информации о лоте аукциона.
/// </summary>
public record AuctionItemDto
{
    /// <summary>
    ///     Уникальный идентификатор лота.
    /// </summary>
    public long Id { get; init; }

    /// <summary>
    ///     Информация о предмете лота.
    /// </summary>
    public LootItemDto? LootItem { get; init; }

    /// <summary>
    ///     Начальная цена лота.
    /// </summary>
    public decimal StartPrice { get; init; }

    /// <summary>
    ///     Минимальный шаг ставки.
    /// </summary>
    public decimal MinPriceStep { get; init; }

    /// <summary>
    ///     Текущая максимальная ставка по лоту.
    /// </summary>
    public AuctionItemBidDto? LastBid { get; set; }

    /// <summary>
    ///     Дата и время создания лота.
    /// </summary>
    public DateTimeOffset CreatedDt { get; init; }

    /// <summary>
    ///     Дата и время окончания аукциона по лоту.
    /// </summary>
    public DateTimeOffset EndDt { get; init; }

    /// <summary>
    ///     Информация о пользователе, предложившем лот.
    /// </summary>
    public UserDto? Creator { get; init; }

    /// <summary>
    ///     Текущий статус лота.
    /// </summary>
    public AuctionItemStatus Status { get; init; }

    /// <summary>
    ///     Описание лота.
    /// </summary>
    public string? Description { get; init; }
}