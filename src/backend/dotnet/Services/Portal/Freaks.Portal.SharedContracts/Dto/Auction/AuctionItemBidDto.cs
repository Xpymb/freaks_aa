using Freaks.Users.SharedContracts.Dto;

namespace Freaks.Portal.SharedContracts.Dto.Auction;

/// <summary>
///     DTO для представления информации о ставке на лот аукциона.
/// </summary>
/// <param name="Id">Уникальный идентификатор ставки.</param>
/// <param name="AuctionItemId">Идентификатор лота аукциона, по которому сделана ставка.</param>
/// <param name="Creator">Информация о пользователе, сделавшем ставку.</param>
/// <param name="Price">Сумма ставки.</param>
/// <param name="CreatedDt">Дата и время создания ставки.</param>
public record AuctionItemBidDto(
    long Id,
    long AuctionItemId,
    UserDto Creator,
    decimal Price,
    DateTimeOffset CreatedDt);