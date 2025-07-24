using System.Net;
using Freaks.WebApi.Common.Exceptions.Base;

namespace Freaks.WebApi.Common.Exceptions.Shop;

/// <summary>
///     Исключение, выбрасываемое при попытке взаимодействия с товаром магазина,
///     который имеет статус <c>ShopItemStatus.Ended</c>.
///     Используется для информирования о том, что товар более недоступен.
/// </summary>
public class ShopItemStatusEndedException : BadRequestApiException
{
    /// <inheritdoc />
    public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;

    /// <inheritdoc />
    public override string ErrorCode => "SHOP_ITEM_STATUS_ENDED";

    /// <summary>
    ///     Инициализирует новый экземпляр <see cref="ShopItemStatusEndedException" />.
    /// </summary>
    /// <param name="message">Сообщение об ошибке.</param>
    /// <param name="innerException">Вложенное исключение (если есть).</param>
    public ShopItemStatusEndedException(string? message = "Shop item already ended", Exception? innerException = null)
        : base(message, innerException)
    {
    }
}