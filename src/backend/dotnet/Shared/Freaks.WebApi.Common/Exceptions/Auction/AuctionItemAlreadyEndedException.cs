using System.Net;
using Freaks.WebApi.Common.Exceptions.Base;

namespace Freaks.WebApi.Common.Exceptions.Auction;

/// <summary>
///     Исключение, которое выбрасывается при попытке совершить операцию над лотом аукциона,
///     который уже завершён.
/// </summary>
public class AuctionItemAlreadyEndedException : BadRequestApiException
{
    /// <inheritdoc />
    public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;

    /// <inheritdoc />
    public override string ErrorCode => "AUCTION_ITEM_ALREADY_ENDED";

    /// <summary>
    ///     Инициализирует новый экземпляр <see cref="AuctionItemAlreadyEndedException" />.
    /// </summary>
    /// <param name="message">Сообщение об ошибке.</param>
    /// <param name="innerException">Вложенное исключение (если есть).</param>
    public AuctionItemAlreadyEndedException(string? message = "Auction by item already ended", Exception? innerException = null)
        : base(message, innerException)
    {
    }
}