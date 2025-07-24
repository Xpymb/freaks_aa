using System.Net;
using Freaks.WebApi.Common.Exceptions.Base;

namespace Freaks.WebApi.Common.Exceptions.Shop;

/// <summary>
///     Исключение, которое выбрасывается, когда запрашиваемое количество товара в магазине
///     превышает доступное. Используется для обработки ошибок бизнес-логики при оформлении заявки.
///     Наследуется от <see cref="BadRequestApiException" /> и возвращает код ошибки
///     <c>SHOP_ITEM_DOES_NOT_HAVE_REQUESTED_QUANTITY</c>.
/// </summary>
public class ShopItemDoesNotHaveRequestedQuantity : BadRequestApiException
{
    /// <inheritdoc />
    public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;

    /// <inheritdoc />
    public override string ErrorCode => "SHOP_ITEM_DOES_NOT_HAVE_REQUESTED_QUANTITY";

    /// <summary>
    ///     Инициализирует новый экземпляр <see cref="ShopItemDoesNotHaveRequestedQuantity" />.
    /// </summary>
    /// <param name="message">Сообщение об ошибке.</param>
    /// <param name="innerException">Вложенное исключение (если есть).</param>
    public ShopItemDoesNotHaveRequestedQuantity(string? message = "Shop item not have requested quantity", Exception? innerException = null)
        : base(message, innerException)
    {
    }
}