using System.Net;
using Freaks.WebApi.Common.Exceptions.Base;

namespace Freaks.WebApi.Common.Exceptions.Auction;

/// <summary>
///     Исключение, выбрасываемое при попытке сделать ставку с ценой,
///     не превышающей текущую максимальную ставку и меньше минимального шага по лоту.
/// </summary>
public class AuctionItemBidPriceShouldBeHigherException : BadRequestApiException
{
    /// <inheritdoc />
    public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;

    /// <inheritdoc />
    public override string ErrorCode => "AUCTION_ITEM_BID_PRICE_SHOULD_BE_HIGHER";

    /// <summary>
    ///     Инициализирует новый экземпляр <see cref="AuctionItemBidPriceShouldBeHigherException" />.
    /// </summary>
    /// <param name="message">Сообщение об ошибке.</param>
    /// <param name="innerException">Вложенное исключение (если есть).</param>
    public AuctionItemBidPriceShouldBeHigherException(string? message = "Auction item bid should be higher", Exception? innerException = null)
        : base(message, innerException)
    {
    }
}