namespace Freaks.Portal.SharedContracts.ValueObjects.Shop;

/// <summary>
///     Статус предмета магазина.
///     Определяет, доступен ли товар для покупки или уже завершён.
/// </summary>
public enum ShopItemStatus
{
    /// <summary>
    ///     Товар активен и доступен для покупки.
    /// </summary>
    Active = 1,

    /// <summary>
    ///     Продажа товара завершена или он более недоступен.
    /// </summary>
    Ended = 2,
}