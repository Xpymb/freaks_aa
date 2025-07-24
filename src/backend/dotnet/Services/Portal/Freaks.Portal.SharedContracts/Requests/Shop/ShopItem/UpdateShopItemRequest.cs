namespace Freaks.Portal.SharedContracts.Requests.Shop.ShopItem;

/// <summary>
///     Запрос на обновление информации о товаре в магазине.
///     Позволяет изменить цену и количество доступного товара.
/// </summary>
/// <param name="Price">Новая цена товара в условных единицах.</param>
/// <param name="Quantity">Новое количество доступных единиц товара.</param>
public record UpdateShopItemRequest(
    int Price,
    int Quantity);